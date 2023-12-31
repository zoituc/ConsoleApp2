﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string haar = "haarcascade_frontalface_default.xml";
            string video = "videos.avi";
                      string path = Directory.GetCurrentDirectory();
            string haarPath = Path.Combine(path, haar);
            string videoPath = Path.Combine(path, video);
            string imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "images");



            if (!File.Exists(videoPath))
            {
                Console.WriteLine("El video no se encuentra en la carpeta.");
                return;
            }

            using (CascadeClassifier cascade = new CascadeClassifier(haarPath))
            {

                using (VideoCapture capture = new VideoCapture(videoPath))
                {

                    if (capture == null)         //se abre el video
                    {
                        Console.WriteLine("No se pudo abrir el video.");
                        return;
                    }

                    if (!Directory.Exists(imagePath))          //creacion de directorio
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    int frameCount = 1;

                    // Bucle para mostrar cada frame del video
                    while (true)
                    {
                        Mat frame = new Mat();
                        Mat frameScale = new Mat();
                        capture.Read(frame);

                             if (frame.Empty())
                            break;

                        var faces=     cascade.DetectMultiScale(frame, 1.3, 5);
                        
                        Cv2.CvtColor(frame, frameScale, ColorConversionCodes.RGB2GRAY);
                       

                        foreach (Rect face in faces)
                        {
                            Cv2.Rectangle(frame, face, Scalar.Red, 2);
                        }

                        string imageName = Path.Combine(imagePath, $"image_{frameCount}.png");
                        frameCount++;

                        Cv2.ImWrite(imageName, frame);   //guardar imagenes en carpeta images del escritorio

                        Cv2.ImShow("Video", frame);             //mostra video
                        Cv2.WaitKey(30);
                    }

                    // Liberar recursos al finalizar
                    Cv2.DestroyAllWindows();
                }
            }
        }
    }
}
