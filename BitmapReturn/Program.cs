using System;
using System.Drawing.Imaging;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace BitmapReturn
{
    class Program
    {
        static void Main(string[] args) // resx, resy
        {
            int ResX;
            int ResY;
            if (args.Length == 2)
            {
                ResX = Int32.Parse(args[0]);
                ResY = Int32.Parse(args[1]);
            }
            else
            {
                ResX = 1920;
                ResY = 1080;
            }
            System.Drawing.Bitmap bitmap;
            Adapter adapter = new Factory1().GetAdapter(0);
            SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
            Output1 output = adapter.GetOutput(0).QueryInterface<Output1>();
            int num = ResX;
            int num2 = ResY;
            Texture2DDescription description = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = num,
                Height = num2,
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
            bool captureDone = false;
            for (int i = 0; !captureDone; i++)
            {
                try
                {
                    SharpDX.DXGI.Resource screenResource;
                    OutputDuplicateFrameInformation duplicateFrameInformation;

                    // Try to get duplicated frame within given time
                    duplicatedOutput.AcquireNextFrame(10000, out duplicateFrameInformation, out screenResource);

                    if (i > 0)
                    {
                        // copy resource into memory that can be accessed by the CPU
                        using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                            device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                        // Get the desktop capture texture
                        var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

                        // Create Drawing.Bitmap
                        bitmap = new System.Drawing.Bitmap(ResX, ResY, PixelFormat.Format32bppArgb);
                        var boundsRect = new System.Drawing.Rectangle(0, 0, ResX, ResY);

                        // Copy pixels from screen capture Texture to GDI bitmap
                        var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        var sourcePtr = mapSource.DataPointer;
                        var destPtr = mapDest.Scan0;
                        for (int y = 0; y < ResY; y++)
                        {
                            // Copy a single line 
                            Utilities.CopyMemory(destPtr, sourcePtr, ResX * 4);

                            // Advance pointers
                            sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                            destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                        }

                        // Release source and dest locks
                        bitmap.UnlockBits(mapDest);
                        device.ImmediateContext.UnmapSubresource(screenTexture, 0);

                        // Save the output
                        Debug.WriteLine("Attempting to save");
                        Thread.Sleep(30);
                        if (File.Exists(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp")))
                        {
                            File.Delete(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp"));
                        }
                        File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp"), "empty");
                        bitmap.Save(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp"));

                        // Capture done
                        captureDone = true;
                    }

                    screenResource.Dispose();
                    duplicatedOutput.ReleaseFrame();

                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                    {
                        throw ex;
                    }
                }
                //adapter.Dispose();
            }
            if (args.Length != 2)
            {
                Process.Start(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "ScreenCapture.bmp")));
            }
        }
    }
}
