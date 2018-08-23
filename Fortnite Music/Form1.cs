using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX;
using System.Drawing.Imaging;
using Microsoft.Win32;
// TO DO:
// if obscured then continue playing // DONE
// work in party
// wait time amount between clicking ok and taking screenshot (in seconds)
namespace Fortnite_Music
{
	public partial class Form1 : Form
	{
		WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();


		[DllImport("user32.dll")]
		static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		public static class Globals
		{
			public static string titlemenu = "";
			public static string mainmenu = "";
			public static string victory = "";
			public static bool stretched = false;
			public const bool writelogs = false; // enable this is for want logs to be created. Else only the initialized message will be written.
			public static double sfx = 1;
			public static double sfy = 1;
			public static bool releasebitmap = false;
			public static bool optimize = Properties.Settings.Default.optimize;
		}

		// P/Invoke declarations
		[DllImport("user32.dll")]
		static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
		[DllImport("gdi32.dll")]
		static extern IntPtr DeleteDC(IntPtr hDc);
		[DllImport("gdi32.dll")]
		static extern IntPtr DeleteObject(IntPtr hDc);
		[DllImport("gdi32.dll")]
		static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
		[DllImport("gdi32.dll")]
		static extern IntPtr CreateCompatibleDC(IntPtr hdc);
		[DllImport("gdi32.dll")]
		static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr ptr);
		//static Adapter adapter = new Factory1().GetAdapter(0);
		//static SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
		private System.Drawing.Bitmap TakeScreenshot(Adapter adapter, SharpDX.Direct3D11.Device device)
		{
			try
			{
				int ResX = Properties.Settings.Default.ResX;
				int ResY = Properties.Settings.Default.ResY;
				writetolog("Got resolutions");
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(ResX, ResY, PixelFormat.Format32bppArgb);
				//
				Output1 output = adapter.GetOutput(0).QueryInterface<Output1>();
				Texture2DDescription description = new Texture2DDescription
				{
					CpuAccessFlags = CpuAccessFlags.Read,
					BindFlags = BindFlags.None,
					Format = Format.B8G8R8A8_UNorm,
					Width = ResX,
					Height = ResY,
					OptionFlags = ResourceOptionFlags.None,
					MipLevels = 1,
					ArraySize = 1,
					SampleDescription =
				{
					Count = 1,
					Quality = 0
				},
					Usage = ResourceUsage.Staging
				};
				Texture2D screenTexture = new Texture2D(device, description);
				OutputDuplication duplicatedOutput = output.DuplicateOutput(device);
				writetolog("setup (variables) complete, entering capture loop");
				bool captureDone = false;
				//
				for (int i = 0; !captureDone; i++)
				{
					//try
					//{
					SharpDX.DXGI.Resource screenResource;
					OutputDuplicateFrameInformation duplicateFrameInformation;
					//writetolog("loop (variables) complete");
					// Try to get duplicated frame within given time
					duplicatedOutput.AcquireNextFrame(10000, out duplicateFrameInformation, out screenResource);

					if (i > 0)
					{
						// copy resource into memory that can be accessed by the CPU
						using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
							device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
						//writetolog("resource");
						// Get the desktop capture texture
						var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);
						//writetolog("source");

						// Create Drawing.Bitmap
						var boundsRect = new System.Drawing.Rectangle(0, 0, ResX, ResY);
						//writetolog("boundrect");
						// Copy pixels from screen capture Texture to GDI bitmap
						var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
						//writetolog("lockbits");
						var sourcePtr = mapSource.DataPointer;
						//writetolog("sourceptr");
						var destPtr = mapDest.Scan0;
						//writetolog("destptr + secondloop");

						for (int y = 0; y < ResY; y++)
						{
							//Debug.WriteLine("Screenshotwait");
							if (Globals.optimize)
							{
								Thread.Sleep(1);
							}
							// Copy a single line 
							Utilities.CopyMemory(destPtr, sourcePtr, ResX * 4);
							writetolog("copmem");
							// Advance pointers
							sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
							destPtr = IntPtr.Add(destPtr, mapDest.Stride);
							writetolog("ptr");
						}
						Debug.WriteLine("Done!!!!!!! SCREENSHOT!!!!!!!");
						// Release source and dest locks
						bitmap.UnlockBits(mapDest);
						writetolog("unlock");
						device.ImmediateContext.UnmapSubresource(screenTexture, 0);

						// Save the output
						writetolog("done");
						Debug.WriteLine("Done");
						//bitmap.Save(@"C:\Users\Aperture\screencap.bmp");
						//
						//return bitmap;


						// Capture done
						captureDone = true;
					}
					writetolog("sr dispose");
					screenResource.Dispose();
					duplicatedOutput.ReleaseFrame();

					//}
					//catch (SharpDXException ex)
					//{
					//	if (ex.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
					//	{
					//		throw ex;
					//	}
					//}
				}
				writetolog("general end dispose");
				output.Dispose();
				screenTexture.Dispose();
				duplicatedOutput.Dispose();
				writetolog("retbitmap");
				return bitmap;
			}
			catch
			{
				Debug.WriteLine("Error detected!!");
				return new System.Drawing.Bitmap(1920, 1080);
			}
			//Process.Start(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp")));
		}
		private System.Drawing.Bitmap GetBitmap(Adapter adapter, SharpDX.Direct3D11.Device device)
		{
			Debug.WriteLine("CALLED!!!!!!");
			//TakeScreenshot();
			while (true)
			{
				//try
				//{
				if (System.IO.File.Exists(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp")))
				{
					System.IO.File.Delete(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp")); // free thread!!
				}
				using (Process pr = new Process())
				{
					//pr.StartInfo.FileName = "BitMapReturn.exe";
					//pr.StartInfo.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"BitMapReturn");
					//pr.StartInfo.Arguments = Properties.Settings.Default.ResX + " " + Properties.Settings.Default.ResY;
					//pr.Start();
					//pr.WaitForExit();
					System.Drawing.Bitmap bMap = TakeScreenshot(adapter, device);
					//GC.Collect();
					//GC.WaitForPendingFinalizers();
					//bMap.Save(@"C:\Users\Aperture\source\repos\Fortnite-Music-Changer\Fortnite Music\bin\Debug\TestImage.bmp");
					Debug.WriteLine("Back!");
					//bMap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp"));
					var cat = GetColorAt(bMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(428f * Globals.sfx)), Convert.ToInt32(Math.Round(548f * Globals.sfy))));
					//Debug.WriteLine("eeeeeeeeeee" + cat);
					if (cat.A != 0)
					{
						pr.Dispose();
						Debug.WriteLine("ReturningBmap");
						return bMap;
						//bMap.Dispose();
						// break;
					}
					else
					{
						pr.Dispose();
						bMap.Dispose();
					}
				}
				//} catch
				//{

				//}
			}
		}
		private bool mainmenumusic(System.Drawing.Bitmap BitMap, double sfx, double sfy)
		{
			writetolog("----- MAIN MENU -----");
			System.Drawing.Color colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(428f * sfx)), Convert.ToInt32(Math.Round(548f * sfy))));
			writetolog(colorAt.ToString());
			if (int.Parse(colorAt.R.ToString()) == 11 && int.Parse(colorAt.G.ToString()) == 19 && int.Parse(colorAt.B.ToString()) == 47)
			{
				return false;
			}
			colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
			Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu2.R + " " + Properties.Settings.Default.menu2.G + " " + Properties.Settings.Default.menu2.B);

			writetolog(colorAt.ToString());
			if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu2.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu2.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu2.B)
			{
				colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
				writetolog(colorAt.ToString());
				Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu3.R + " " + Properties.Settings.Default.menu3.G + " " + Properties.Settings.Default.menu3.B);
				if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu3.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu3.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu3.B)
				{
					if (Properties.Settings.Default.PlayInParty == false)
					{
						colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
						writetolog(colorAt.ToString());
						Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu4.R + " " + Properties.Settings.Default.menu4.G + " " + Properties.Settings.Default.menu4.B);
						if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) >= Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) >= Properties.Settings.Default.menu4.B)
						{
							return true;
						}
						if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu4.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu4.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu4.B)
						{
							return true;
						}
					} else
					{
						return true;
					}
				}
			}
			Debug.WriteLine("----------------------------------- SETTINGS");
			colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
			Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu5.R + " " + Properties.Settings.Default.menu5.G + " " + Properties.Settings.Default.menu5.B);
			if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu5.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu5.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu5.B)
			{
				colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
				Debug.WriteLine(colorAt + " " + Properties.Settings.Default.menu6.R + " " + Properties.Settings.Default.menu6.G + " " + Properties.Settings.Default.menu6.B);
				if (int.Parse(colorAt.R.ToString()) == Properties.Settings.Default.menu6.R && int.Parse(colorAt.G.ToString()) == Properties.Settings.Default.menu6.G && int.Parse(colorAt.B.ToString()) == Properties.Settings.Default.menu6.B)
				{
					return true;
				}
			}
			return false;
		}
		private bool victorymusic(System.Drawing.Bitmap BitMap, double sfx, double sfy)
		{
			writetolog("----- VICTORY -----");
			if (Properties.Settings.Default.stretched == false)
			{
				System.Drawing.Color colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(911f * sfx)), Convert.ToInt32(Math.Round(251f * sfy))));
				writetolog(colorAt.ToString());
				if (int.Parse(colorAt.R.ToString()) >= 242 && int.Parse(colorAt.G.ToString()) >= 247 && int.Parse(colorAt.B.ToString()) >= 252)
				{
					colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1087f * sfx)), Convert.ToInt32(Math.Round(271f * sfy))));
					writetolog(colorAt.ToString());

					if (int.Parse(colorAt.R.ToString()) >= 255 && int.Parse(colorAt.G.ToString()) >= 255 && int.Parse(colorAt.B.ToString()) >= 255)
					{
						return true;
					}
				}
				return false;
			}
			else
			{
				System.Drawing.Color colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(682f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(344f * (Properties.Settings.Default.ResY / 1080.0)))));
				writetolog(colorAt.ToString());
				if (int.Parse(colorAt.R.ToString()) >= 253 && int.Parse(colorAt.G.ToString()) >= 253 && int.Parse(colorAt.B.ToString()) >= 253)
				{
					colorAt = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(808f * (Properties.Settings.Default.ResX / 1440.0))), Convert.ToInt32(Math.Round(242f * (Properties.Settings.Default.ResY / 1080.0)))));
					writetolog(colorAt.ToString());

					if (int.Parse(colorAt.R.ToString()) >= 219 && int.Parse(colorAt.G.ToString()) >= 253 && int.Parse(colorAt.B.ToString()) >= 245)
					{
						return true;
					}
				}
				return false;
			}
		}
		private void writetolog(string towrite)
		{
			if (Globals.writelogs == true)
			{
				while (true)
				{
					try
					{
						System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\log.txt") + towrite + System.Environment.NewLine);
						break;
					}
					catch
					{

					}
				}
			}
		}
		private void deletebitmap()
		{
		}
		private void SetStartup()
		{
			RegistryKey rk = Registry.CurrentUser.OpenSubKey
				("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

			if (launchOnStartupToolStripMenuItem.Checked)
			{
				Properties.Settings.Default.Startup = true;
				Properties.Settings.Default.Save();
				Properties.Settings.Default.Reload();
				rk.SetValue("Fortnite Music", Application.ExecutablePath);
			}
			else
			{
				Properties.Settings.Default.Startup = false;
				Properties.Settings.Default.Save();
				Properties.Settings.Default.Reload();
				rk.DeleteValue("Fortnite Music", false);
			}

		}
		public Form1()
		{
			Adapter adapter = new Factory1().GetAdapter(0);
			SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
			string tag = "2.1.4";
			InitializeComponent();
			// minimized
			if (Properties.Settings.Default.StartMinimized)
			{
				this.WindowState = FormWindowState.Minimized;
				startMinimizedToolStripMenuItem.Checked = true;
			}
			// first startup

			// Auto update
			string html;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/ApertureC/Fortnite-Music-Changer/releases/latest?UserAgent=hi");
			request.ContentType = "application/json";
			request.UserAgent = "e";
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				html = reader.ReadToEnd();
			}
			dynamic data = JObject.Parse(html);
			if (data.tag_name != tag)
			{
				if (Microsoft.VisualBasic.Interaction.MsgBox("An update is available (" + data.name + "), would you like to view the latest version?", Microsoft.VisualBasic.MsgBoxStyle.YesNo, "Update") == Microsoft.VisualBasic.MsgBoxResult.Yes)
				{
					Debug.WriteLine("going");
					System.Diagnostics.Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer/releases/latest");
				}
			}
			Debug.WriteLine(html);
			// SETTINGS LOADING
			//var DPI=(int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "LogPixels", 96);
			//var scale = 96 / (float)DPI;
			System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "\\log.txt", "Initizalized!" + System.Environment.NewLine); // to do create logs //
																																		  // to do: just ask for resolution
			Debug.WriteLine(Properties.Settings.Default.ResX);
			if (Properties.Settings.Default.ResX == 0)
			{
				while (true)
				{
					string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "1920", 0, 0);
					try
					{
						int ix = Convert.ToInt32(x);
						Properties.Settings.Default.ResX = ix;
						Properties.Settings.Default.Save();
						Properties.Settings.Default.Reload();
						break;
					}
					catch
					{
						Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
					}
				}
				while (true)
				{
					string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "1080", 0, 0);
					try
					{
						int iy = Convert.ToInt32(y);
						Properties.Settings.Default.ResY = iy;
						Properties.Settings.Default.Save();
						Properties.Settings.Default.Reload();
						break;
					}
					catch
					{
						Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
					}
				}
			}
			var sfx = Properties.Settings.Default.ResX / 1920.0;
			var sfy = Properties.Settings.Default.ResY / 1080.0;
			//
			Globals.sfx = sfx;
			Globals.sfy = sfy;
			if (Properties.Settings.Default.title1 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0) || Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
			{
				int waittime;
				while (true)
				{
					string wts = Microsoft.VisualBasic.Interaction.InputBox("Enter the wait time (in seconds) between clicking OK and taking a screenshot **IF YOU EXPERIENCE ISSUES WITH MUSIC PLAYING PRESS 'MENU SETUP' AND INCREASE THE TIME AND TRY AGAIN**", "Wait Time", "2", 0, 0);
					try
					{
						waittime = Convert.ToInt32(wts);
						break;
					}
					catch
					{
						Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
					}
				}
				Debug.WriteLine(Properties.Settings.Default.title1);
				if (Properties.Settings.Default.title1 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
				{
					Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
					var done = false;
					while (true)
					{
						if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
						{
							Thread.Sleep(waittime*1000);
							System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
							//System.Drawing.Bitmap BitMap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp"));
							//System.Drawing.Bitmap BitMap = GetBitmap();
							Debug.WriteLine("Passed" + BitMap);
							//BitMap.Save(@"C:\Users\Aperture\source\repos\Fortnite-Music-Changer\Fortnite Music\bin\Debug\SaveImage2.bmp");
							uint pid;
							GetWindowThreadProcessId(GetForegroundWindow(), out pid);
							Debug.WriteLine(GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(428f * Globals.sfx)), Convert.ToInt32(Math.Round(548f * Globals.sfy)))));
							if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
							{
								foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
								{
									if (p.Id == pid)
									{
										Debug.WriteLine("CURRENTLY FOREGROUND: " + p.ProcessName)
											;
										if (p.ProcessName == "FortniteClient-Win64-Shipping")
										{
											Debug.WriteLine("Passed2");
											Properties.Settings.Default.title1 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
											Properties.Settings.Default.title2 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
											Properties.Settings.Default.Save();
											Properties.Settings.Default.Reload();
											done = true;
										}

									}
								}
							}
							BitMap.Dispose();
							deletebitmap();
						}
						if (done == true)
						{
							this.Activate();

							Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
							break;
						}
					}
				}
				if (Properties.Settings.Default.menu2 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
				{
					Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the BR menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
					var done = false;
					while (true)
					{
						if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
						{
							Thread.Sleep(waittime * 1000);
							System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
							Debug.WriteLine("!!!" + GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))));
							uint pid;
							GetWindowThreadProcessId(GetForegroundWindow(), out pid);
							if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
							{

								foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
								{
									if (p.Id == pid)
									{
										if (p.ProcessName == "FortniteClient-Win64-Shipping")
										{
											Properties.Settings.Default.menu2 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
											Properties.Settings.Default.menu3 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
											Properties.Settings.Default.menu4 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
											Properties.Settings.Default.Save();
											Properties.Settings.Default.Reload();
											done = true;
										}

									}
								}
							}
							BitMap.Dispose();
							deletebitmap();
						}
						if (done == true)
						{
							this.Activate();
							Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");

							break;
						}
					}
				}
				if (Properties.Settings.Default.menu5 == System.Drawing.Color.FromArgb(0, 0, 0, 0))
				{
					Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Settings menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
					var done = false;
					while (true)
					{
						if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
						{
							Debug.WriteLine("STUFF1");
							Thread.Sleep(waittime * 1000);
							System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
							//System.Drawing.Bitmap BitMap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp"));
							uint pid;
							GetWindowThreadProcessId(GetForegroundWindow(), out pid);
							if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
							{

								foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
								{
									if (p.Id == pid)
									{
										if (p.ProcessName == "FortniteClient-Win64-Shipping")
										{
											Debug.WriteLine("STUFF2");
											Properties.Settings.Default.menu5 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
											Properties.Settings.Default.menu6 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
											Properties.Settings.Default.Save();
											Properties.Settings.Default.Reload();
											done = true;
										}

									}
								}
							}
							BitMap.Dispose();
							deletebitmap();
						}
						if (done == true)
						{
							this.Activate();
							Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
							break;
						}
					}
				}
				Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
				Application.Exit();
			}
			writetolog("Scale factor X: " + sfx.ToString());
			writetolog("Scale factor Y: " + sfy.ToString());
			//
			writetolog("Resolution X " + Properties.Settings.Default.ResX);
			writetolog("Resolution Y " + Properties.Settings.Default.ResY);
			int currentlyplaying = 0; // 0=nothing 1=title 2=menu 3=victory
									  //
									  //while (true)
									  //{
									  //    Debug.WriteLine(System.Windows.Forms.Cursor.Position.ToString());
									  //    Debug.WriteLine(GetColorAt(BitMap,new Point(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y)).ToString());
									  //}
									  // SETTINGS

			Debug.WriteLine("Loaded");
			Globals.mainmenu = Properties.Settings.Default.MainMenu;
			Globals.victory = Properties.Settings.Default.Victory;
			Globals.stretched = Properties.Settings.Default.stretched;
			Globals.titlemenu = Properties.Settings.Default.TitleMenu;
			Globals.optimize = Properties.Settings.Default.optimize;
			writetolog("Menu " + Globals.mainmenu);
			writetolog("Title " + Globals.titlemenu);
			writetolog("Victory " + Globals.victory);
			checkBox1.Checked = Properties.Settings.Default.Obscure;
			checkBox2.Checked = Properties.Settings.Default.stretched;
			checkBox3.Checked = Properties.Settings.Default.optimize;
			launchOnStartupToolStripMenuItem.Checked = Properties.Settings.Default.Startup;

			trackBar1.Value = Properties.Settings.Default.Volume;

			// APPLY SETTINGS
			MenuMusicFile.Text = Globals.mainmenu;
			TitleMenuFile.Text = Globals.titlemenu;
			VictoryMusicFile.Text = Globals.victory;
			//
			Thread t = new Thread(() =>
			{
				wplayer.settings.setMode("loop", true);
				wplayer.settings.setMode("autoStart", false);
				Thread.CurrentThread.IsBackground = true;

				while (true)
				{
					Debug.WriteLine("Starting Loop");
					Thread.Sleep(250);//250

					//
					GC.Collect();
					GC.WaitForPendingFinalizers();
					//
					writetolog("----- NEW CYCLE -----");
					MethodInvoker mouse = delegate
					{
						label1.Text = System.Windows.Forms.Cursor.Position.ToString();
						using (System.Drawing.Bitmap BitMap = GetBitmap(adapter, device))
						{

							label1.Text = System.Windows.Forms.Cursor.Position.ToString() + " " + GetColorAt(BitMap, new System.Drawing.Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y)).ToString();
							//label1.Text = System.Windows.Forms.Screen.PrimaryScreen;
							//label2.Text = GetColorAt(BitMap,);
							return;
						}
					};
					//this.Invoke(mouse);
					if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
					{
						bool focused = false;
						uint pid;
						GetWindowThreadProcessId(GetForegroundWindow(), out pid);
						foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
						{
							if (p.Id == pid)
							{
								if (p.ProcessName == "FortniteClient-Win64-Shipping")
								{
									focused = true;
									break;
								}

							}
						}
						writetolog("focus: " + focused.ToString());
						Debug.WriteLine("Changing BitMap");
						if (focused == true)
						{
							if (Globals.releasebitmap == false)
							{
								try
								{
									System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
									//System.Drawing.Bitmap BitMap = new System.Drawing.Bitmap(1920, 1080);
									// REPLACE ERRORS WITH THIS Path.Combine(Environment.CurrentDirectory, @"BitmapReturn\ScreenCapture.bmp")
									Debug.WriteLine("Passed");
									var c = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
									writetolog(c.ToString());
									Debug.WriteLine(c);
									Debug.WriteLine(c + Properties.Settings.Default.title1.R.ToString() + Properties.Settings.Default.title1.G.ToString() + Properties.Settings.Default.title1.B.ToString());
									if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title1.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title1.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title1.B)
									{
										c = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
										writetolog(c.ToString());
										Debug.WriteLine(c);
										if (Int32.Parse(c.R.ToString()) == Properties.Settings.Default.title2.R && Int32.Parse(c.G.ToString()) == Properties.Settings.Default.title2.G && Int32.Parse(c.B.ToString()) == Properties.Settings.Default.title2.B)
										{

											writetolog("Started playing title menu");

											if ((currentlyplaying != 1 || wplayer.URL != Globals.titlemenu))
											{
												currentlyplaying = 1;
												wplayer.controls.stop();
												wplayer.URL = Globals.titlemenu;
											}
											wplayer.controls.play();
										}
									}
									else if (mainmenumusic(BitMap, sfx, sfy) == true) // to do stop thread on menu setup + changing resolution
									{
										writetolog("Started playing main menu");
										if ((currentlyplaying != 2 || wplayer.URL != Globals.mainmenu))
										{
											currentlyplaying = 2;
											wplayer.controls.stop();
											wplayer.URL = Globals.mainmenu;
										}
										wplayer.controls.play();
									}
									else if (victorymusic(BitMap, sfx, sfy) == true)
									{
										writetolog("Started playing victory");
										if ((currentlyplaying != 3 || wplayer.URL != Globals.victory))
										{
											currentlyplaying = 3;
											wplayer.controls.stop();
											wplayer.URL = Globals.victory;
										}
										wplayer.controls.play();
									}
									else
									{
										if (checkBox1.Checked == false)
										{
											Debug.WriteLine("STOPHERE");
											wplayer.controls.pause();
											//wplayer.URL = "";
										}
										else if (focused == true)
										{
											Debug.WriteLine("STOPHERE2");
											wplayer.controls.pause();
										}
									}
									BitMap.Dispose();
								}
								catch
								{
									Debug.WriteLine("Hi");
								}
							}
						}
						else
						{
							if (checkBox1.Checked==false)
							{
								wplayer.controls.pause();
							}
						}
					}
					else
					{
						writetolog("Fortnite not open");
						wplayer.controls.pause();
						wplayer.URL = "";
					}
				}
			});
			t.IsBackground = true;
			t.Name = "MainThread";
			t.Start();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			Debug.WriteLine("open");
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Globals.titlemenu = openFileDialog1.FileName;
				TitleMenuFile.Text = openFileDialog1.FileName;
				Properties.Settings.Default.TitleMenu = openFileDialog1.FileName;
				Properties.Settings.Default.Save();
				Properties.Settings.Default.Reload();
				Debug.WriteLine("Hi");
			}
		}
		private class User32
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct Rect
			{
				public int left;
				public int top;
				public int right;
				public int bottom;
			}

			[DllImport("user32.dll")]
			public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
		}

		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, uint nFlags);

		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr handle, out System.Drawing.Rectangle rect);

		[DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
		static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
		//public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
		System.Drawing.Bitmap screenPixel = new System.Drawing.Bitmap(1920, 1080, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		public static System.Drawing.Color GetColorAt(System.Drawing.Bitmap BitMap, System.Drawing.Point location)
		{
			try
			{
				int x = location.X;
				int y = location.Y;

				return BitMap.GetPixel(x, y);
			}
			catch
			{
				return System.Drawing.Color.FromArgb(0, 0, 0, 0);
			}
		}
		//
		private void button2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Globals.mainmenu = openFileDialog1.FileName;
				MenuMusicFile.Text = openFileDialog1.FileName;
				Properties.Settings.Default.MainMenu = openFileDialog1.FileName;
				Properties.Settings.Default.Save();
				Properties.Settings.Default.Reload();
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Obscure = checkBox1.Checked;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();

		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			wplayer.settings.volume = trackBar1.Value;
			VolumeNum.Text = trackBar1.Value.ToString();
			Properties.Settings.Default.Volume = trackBar1.Value;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();

		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Globals.victory = openFileDialog1.FileName;
				VictoryMusicFile.Text = openFileDialog1.FileName;
				Properties.Settings.Default.Victory = openFileDialog1.FileName;
				Properties.Settings.Default.Save();
				Properties.Settings.Default.Reload();
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.stretched = checkBox2.Checked;
			Globals.stretched = checkBox2.Checked;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			while (true)
			{
				string x = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) X Resolution", "1920", 0, 0);
				try
				{
					int ix = Convert.ToInt32(x);
					Properties.Settings.Default.ResX = ix;
					Properties.Settings.Default.Save();
					Properties.Settings.Default.Reload();
					break;
				}
				catch
				{
					Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
				}
			}
			while (true)
			{
				string y = Microsoft.VisualBasic.Interaction.InputBox("Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "Please enter your monitors OR FULLSCREEN (STRETCHED) Y Resolution", "1080", 0, 0);
				try
				{
					int iy = Convert.ToInt32(y);
					Properties.Settings.Default.ResY = iy;
					Properties.Settings.Default.Save();
					Properties.Settings.Default.Reload();
					break;
				}
				catch
				{
					Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
				}
			}
			Microsoft.VisualBasic.Interaction.MsgBox("Please Restart the program", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart required");

		}

		private void button4_Click_1(object sender, EventArgs e)
		{
			Adapter adapter = new Factory1().GetAdapter(0);
			SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
			Globals.releasebitmap = true;
			Thread.Sleep(500);
			var sfx = Globals.sfx;
			var sfy = Globals.sfy;
			int waittime;
			while (true)
			{
				string wts = Microsoft.VisualBasic.Interaction.InputBox("Enter the wait time (in seconds) between clicking OK and taking a screenshot **IF YOU EXPERIENCE ISSUES WITH MUSIC PLAYING PRESS 'MENU SETUP' AND INCREASE THE TIME AND TRY AGAIN**", "Wait Time", "2", 0, 0);
				try
				{
					waittime = Convert.ToInt32(wts);
					break;
				}
				catch
				{
					Microsoft.VisualBasic.Interaction.MsgBox("The value you entered was not a valid value. Please enter a number", Microsoft.VisualBasic.MsgBoxStyle.Information, "Invalid value");
				}
			}
			Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the title screen (STW or BR selection) on fortnite" + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
			var done = false;
			while (true)
			{
				if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
				{
					Thread.Sleep(waittime*1000);
					System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
					uint pid;
					GetWindowThreadProcessId(GetForegroundWindow(), out pid);
					if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
					{
						foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
						{
							if (p.Id == pid)
							{
								if (p.ProcessName == "FortniteClient-Win64-Shipping")
								{
									Properties.Settings.Default.title1 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy))));
									Properties.Settings.Default.title2 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(985 * sfx)), Convert.ToInt32(Math.Round(780 * sfy))));
									Properties.Settings.Default.Save();
									Properties.Settings.Default.Reload();
									done = true;
								}

							}
						}
					}
					BitMap.Dispose();
					deletebitmap();
				}
				if (done == true)
				{
					this.Activate();

					Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
					break;
				}
			}
			Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the BR menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
			done = false;
			while (true)
			{
				if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
				{
					Thread.Sleep(waittime * 1000);
					System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
					uint pid;
					GetWindowThreadProcessId(GetForegroundWindow(), out pid);
					if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
					{

						foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
						{
							if (p.Id == pid)
							{
								if (p.ProcessName == "FortniteClient-Win64-Shipping")
								{
									Properties.Settings.Default.menu2 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(512f * sfx)), Convert.ToInt32(Math.Round(36f * sfy))));
									Properties.Settings.Default.menu3 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(909f * sfx)), Convert.ToInt32(Math.Round(1047f * sfy))));
									Properties.Settings.Default.menu4 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(20f * sfx)), Convert.ToInt32(Math.Round(1043f * sfy))));
									Properties.Settings.Default.Save();
									Properties.Settings.Default.Reload();
									done = true;
								}

							}
						}
					}
					BitMap.Dispose();
					deletebitmap();
				}
				if (done == true)
				{
					this.Activate();
					Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
					break;
				}
			}
			Microsoft.VisualBasic.Interaction.MsgBox("1. Go to the Settings menu on fortnite " + Environment.NewLine + "2. Press Ok on this message" + Environment.NewLine + "3.Click back onto fortnite", Microsoft.VisualBasic.MsgBoxStyle.Information, "Sampling");
			done = false;
			while (true)
			{
				if (Process.GetProcessesByName("FortniteClient-Win64-Shipping").Length > 0)
				{
					Thread.Sleep(waittime * 1000);
					System.Drawing.Bitmap BitMap = GetBitmap(adapter, device);
					uint pid;
					GetWindowThreadProcessId(GetForegroundWindow(), out pid);
					if (GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1058 * sfx)), Convert.ToInt32(Math.Round(28 * sfy)))).A != 0)
					{
						foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
						{
							if (p.Id == pid)
							{
								if (p.ProcessName == "FortniteClient-Win64-Shipping")
								{
									Properties.Settings.Default.menu5 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1897f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
									Properties.Settings.Default.menu6 = GetColorAt(BitMap, new System.Drawing.Point(Convert.ToInt32(Math.Round(1825f * sfx)), Convert.ToInt32(Math.Round(10f * sfy))));
									Properties.Settings.Default.Save();
									Properties.Settings.Default.Reload();
									done = true;
								}

							}
						}
					}
					BitMap.Dispose();
					deletebitmap();
				}
				if (done == true)
				{
					this.Activate();
					Microsoft.VisualBasic.Interaction.MsgBox("Done", Microsoft.VisualBasic.MsgBoxStyle.Information, "Done");
					break;
				}
			}
			Microsoft.VisualBasic.Interaction.MsgBox("Please restart the application (no idea why but it doesn't work without a restart)", Microsoft.VisualBasic.MsgBoxStyle.Information, "Restart");
			Application.Exit();
		}

		private void licensesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Microsoft.VisualBasic.Interaction.MsgBox(@"(C) All rights reserved. 

However you CAN distribute releases FROM THIS REPOSITORIES RELEASE PAGE (https://github.com/ApertureC/Fortnite-Music-Changer/releases) on any hosting platform, as well as use it in videos and on social media.


Message me on reddit /u/ApertureCoder", Microsoft.VisualBasic.MsgBoxStyle.Information, "License");
			Microsoft.VisualBasic.Interaction.MsgBox(@"Copyright (c) 2010-2015 SharpDX - Alexandre Mutel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the 'Software'), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

            The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.", Microsoft.VisualBasic.MsgBoxStyle.Information, "SharpDX");
			Microsoft.VisualBasic.Interaction.MsgBox(@"The MIT License (MIT)

Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the 'Software'), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and / or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

            The above copyright notice and this permission notice shall be included in all
            copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.", Microsoft.VisualBasic.MsgBoxStyle.Information, "Newtonsoft.Json");
		}
		private void githubToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/ApertureC/Fortnite-Music-Changer");
		}

		private void redditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("https://reddit.com/u/ApertureCoder");
		}

		private void launchOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetStartup();
		}

		private void startMinimizedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.StartMinimized = startMinimizedToolStripMenuItem.Checked;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.optimize = checkBox3.Checked;
			Globals.optimize = checkBox3.Checked;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.PlayInParty = checkBox4.Checked;
			Globals.optimize = checkBox4.Checked;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}
	}
}
// Source Repository: https://github.com/ApertureC/Fortnite-Music-Changer