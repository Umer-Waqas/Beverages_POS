using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using System.IO;
using System.Net.Http.Headers;
using Restaurant_MS_Core;

namespace Utilities
{
    public class Service
    {
        private string Email = "DesktopMails@healthwire.pk";
        private string Password = "12345678";
        public string mainURL = "";
        public string Token;
        public Service()
        {
            if (SharedVariables.IsTesting)
            {
                mainURL = "https://api.healthwire.pk";
            }
            else
            {
                mainURL = "https://healthwire.pk";
            }
        }
        public string loginURL = "/api/v2/users/login/";
        public string SendEmailUri = "/api/v2/users/update-results";
        //public string UpdateReportURL = "/api/v2/employees/update_employee";
        //public string AttendanceURL = "/api/v2/attendances";

        public string Login()
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(mainURL);
                var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("user[email]", Email),
                new KeyValuePair<string, string>("user[password]", Password)
                });
                var result = client.PostAsync(loginURL, content).Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }


        public string SendLowStocckEmail(List<string> Receivers, string Token, string FilePath)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(FilePath));
                    fileContent.Headers.ContentDisposition =
                            new ContentDispositionHeaderValue("form-data") //<- 'form-data' instead of 'attachment'
                            {
                                Name = "attachment", // <- included line...
                                FileName = "Foo.txt",
                            };
                    content.Add(fileContent);
                    var resp = client.PostAsync(SendEmailUri, content).Result;
                    var res = resp.Content.ReadAsStringAsync().Result;
                    return res;
                }
            }
            //using (var content = new MultipartFormDataContent())
            //{
            //    var stream = new StreamContent(File.Open(FilePath, FileMode.Open));

            //    stream.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //    stream.Headers.Add("Content-Disposition", "form-data; name=\"import\"; filename=\"LowStocks.xslx\"");
            //    content.Add(stream, "import", "attendances.xslx");
            //    HttpClient client = new HttpClient();
            //    var response = client.PostAsync(SendEmailPath, content).Result;
            //    var result = response.Content.ReadAsStringAsync().Result;
            //    return result;
            //}
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(mainURL);
            //    client.DefaultRequestHeaders.Add("x-user-email", email);
            //    client.DefaultRequestHeaders.Add("x-user-token", token);
            //    //HttpContent content = new FormUrlEncodedContent(new[]{
            //    // new KeyValuePair<string, string>("practice_id", vm.practice_id),
            //    // new KeyValuePair<string, laboratory_results>("laboratory_results", lr),
            //    // new KeyValuePair<string, string>("employee[machine_serial_no]", emp.Machine_Serial_No),
            //    // //new KeyValuePair<string, string>("employee[gender]", emp.Gender.ToString()),
            //    // //new KeyValuePair<string, string>("employee[cnic]", emp.CNIC),
            //    // });
            //    HttpResponseMessage result = new HttpResponseMessage();
            //    result = client.PostAsJsonAsync(AddReportURL,vm).Result;
            //    resultContent = result.Content.ReadAsStringAsync().Result;
            //    return resultContent;
            //}
        }

        //public string UpdateEmployees(DeviceUser emp, string email, string token)
        //{
        //    string resultContent;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(mainURL);
        //        client.DefaultRequestHeaders.Add("x-user-email", email);
        //        client.DefaultRequestHeaders.Add("x-user-token", token);
        //        HttpContent content = new FormUrlEncodedContent(new[] {
        //         new KeyValuePair<string, string>("employee[employee_name]", emp.Name),
        //         new KeyValuePair<string, string>("employee[desktop_id]", emp.EnrollNumber.ToString()),
        //         new KeyValuePair<string, string>("employee[machine_serial_no]", emp.Machine_Serial_No),
        //         //new KeyValuePair<string, string>("employee[gender]", emp.Gender.ToString()),
        //         //new KeyValuePair<string, string>("employee[cnic]", emp.CNIC),
        //         });
        //        HttpResponseMessage result = new HttpResponseMessage();
        //        result = client.PutAsync(UpdateEmployeeURL, content).Result;
        //        resultContent = result.Content.ReadAsStringAsync().Result;
        //        return resultContent;
        //    }
        //}
    }
}