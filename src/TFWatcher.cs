using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace tf2butt.src
{
    internal class TFWatcher
    {
        public string line = "";
        bool uberTriggerAllowed = true; //whether vibration is allowed to trigger
        public void CheckStatus()
        {
            Console.WriteLine("checkstatus");
            using (StreamReader reader = new StreamReader(new FileStream(@"C:\\Program Files (x86)\Steam\steamapps\common\Team Fortress 2\tf\cfg\tf2butt\tf2butt.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                //start at the end of the file
                long lastMaxOffset = reader.BaseStream.Length;

                while (true)
                {

                    System.Threading.Thread.Sleep(10);

                    //if the file size has not changed, idle
                    if (reader.BaseStream.Length == lastMaxOffset)
                        continue;

                    //seek to the last max offset
                    reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                    //read out of the file until the EOF

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("kira killed"))
                        {
                            Console.WriteLine("buttplug should go off");
                            ButtplugConnection connection = new ButtplugConnection();
                            connection.ButtplugVibrate().Wait();
                        }
                        else if (line.Contains("killed kira"))
                        {
                            Console.WriteLine("buttplug should go off");
                            ButtplugConnection connection = new ButtplugConnection();
                            connection.ButtplugVibrate().Wait();
                        }
                        else if(line.Contains("tf2butt_uberactive"))
                        {
                            Console.WriteLine("checking uber percentage");
                            ButtplugConnection connection = new ButtplugConnection();
                            if (uberTriggerAllowed && ReadUber() > 95)
                            {
                                Console.WriteLine("uber high enough, enjoy :)");
                                connection.ButtplugVibrate().Wait();
                                uberTriggerAllowed = false;
                                UberToZero();
                            }
                            else Console.WriteLine("uber percentage not high enough, you must work harder..");
                        }
                    }
                        

                    //update the last max offset
                    lastMaxOffset = reader.BaseStream.Position;
                }
            }
        }
        public void UberToZero()
        {
            //this is used to wait until uber is at 0% before 

            while(true)
            {
                Thread.Sleep(100);
                if (ReadUber() < 8)
                {
                    uberTriggerAllowed = true;
                    return;
                }


            }
        }
        public int ReadUber()
        {


            //take a screenshot of uber
            Rectangle rect = new Rectangle(930, 550, 80, 50);
            Bitmap bmp = new(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            bmp.Save(@".\uberscreenshot.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(@".\uberscreenshot.jpg"))
                    {


                        Page page = engine.Process(img);
                        string replaced = Regex.Replace(page.GetText(), @"[^0-9]", "");
                        try
                        {
                            int converted = Convert.ToInt32(replaced);
                            Console.WriteLine("ocr text: " + replaced);
                            return converted;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("error:" +  e.Message);
                        }
                        return 999; //errror handing?? idfk????
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());

                return 999; //errror handing?? idfk????
            }

        }



    
    }
}
