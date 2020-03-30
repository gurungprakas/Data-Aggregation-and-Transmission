using CSV.Models;
using CSV.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace CSV
{
    class Program
    {
        static void Main(string[] args)

        {
            List<Student> students = new List<Student>();


            List <string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);

            foreach( var directory in directories)
            {
                Student student = new Student() { AbsoulteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);

                Console.WriteLine(student);
                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.Infofile;
                bool fileExists = FTP.FileExists(infoFilePath);

                if(fileExists == true)
                {
                    string csvPath = $@"C:\Users\12499\Desktop\Student Files\{directory}.csv";

                    //FTP.DownloadFile(infoFilePath, csvPath);

                    byte[] bytes = FTP.DownloadFileBytes(infoFilePath);
                    string csvData = Encoding.Default.GetString(bytes);

                    string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                    if(csvlines.Length != 2)
                    {
                        Console.WriteLine("Error in CSV Format!!");
                    }
                    else
                    {
                        student.FromCSV(csvlines[1]);
                    }
                   

                    Console.WriteLine("Found Info File!!");
                }
                else
                {
                    Console.WriteLine("Couldn't find info file:");  
                }

                Console.WriteLine("\t" + infoFilePath);

                string imageFilePath = student.FullPathUrl + "/" + Constants.Locations.Imagefile;
                bool imagefileExists = FTP.FileExists(imageFilePath);

                if (imagefileExists == true)
                {
                    Console.WriteLine("Found Image File!!");
                }
                else
                {
                    Console.WriteLine("Couldn't find Image file:");
                }

                Console.WriteLine("\t" + imageFilePath);

                students.Add(student);

                //Console.WriteLine(directory);
            }

            //Save to CSV
            string studentCSVPath = $"{Constants.Locations.DataFolder}\\students.csv";

            //Establish a file stream to collect data from the response
            using (StreamWriter fs = new StreamWriter(studentCSVPath))
            {
                foreach(var student in students)
                {
                    fs.Write(student.ToCSV());
                }
            }
            return;
            

            //string filePath = @"C:\Users\12499\Desktop\info.csv";
            //string filePath = $@"{Constants.Locations.DataFolder}\{Constants.Locations.Infofile}";
            //string fileContents;

            //using (StreamReader stream = new StreamReader(filePath))
            //{
            //    fileContents = stream.ReadToEnd();
            //}

            //List<string> entries = new List<string>();

            //entries = fileContents.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

            //Student student = new Student();
            //student.FromCSV(entries[1]);



            //string[] data = entries[1].Split(",", StringSplitOptions.None);

            //Student student = new Student();
            //student.StudentId = data[0];
            //student.FirstName = data[1];
            //student.LastName = data[2];
            //student.DateOfBirth = data[3];
            //student.ImageData = data[4];

            //Console.WriteLine(student.ToCSV());
            //Console.WriteLine(student.ToString());




            //string imagefilePath = $"C:\\Users\\12499\\Desktop\\myimage.jpg";
            //string imagefilePath = $"{Constants.Locations.ImagesFolder}\\{Constants.Locations.Imagefile}";
            //Image image = Image.FromFile(imagefilePath);
            //string base64Image = Imaging.ImageToBase64(image, ImageFormat.Jpeg);
            //student.ImageData = base64Image;

            ////string newfilePath = $"C:\\Users\\12499\\Desktop\\{student.ToString()}.jpg";
            //string newfilePath = $"{Constants.Locations.DesktopPath}\\{student.ToString()}.jpg";
            //FileInfo newfileinfo = new FileInfo(newfilePath);
            //Image studentImage = Imaging.Base64ToImage(student.ImageData);
            //studentImage.Save(newfileinfo.FullName, ImageFormat.Jpeg);
        }

        

    }
}
