using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class HttpClient
{
    public HttpClient()
    {
    }

    public class WebResponse
    {
        public HttpStatusCode HttpStatusCode;
        public string body;
    }

    private void setHeaders(HttpWebRequest _request, Dictionary<string, string> _dict)
    {
        foreach (KeyValuePair<string, string> keyValuePair in _dict)
            _request.Headers[keyValuePair.Key] = keyValuePair.Value;

        _request.ContentType = "application/x-www-form-urlencoded";
        _request.Accept = "application/json";
    }

    private async Task<string> responseStream(HttpWebResponse _response)
    {
        string data;

        using (var responseStream = _response.GetResponseStream())
        {
            using (var postStreamReader = new StreamReader(responseStream))
            {
                data = await postStreamReader.ReadToEndAsync();
                postStreamReader.Close();
            }
            responseStream.Close();
        }

        return data ?? string.Empty;
    }
    public async Task<WebResponse> post(string _url, Dictionary<string, string> _headers, string _postData)
    {
        var request = (HttpWebRequest)WebRequest.Create(_url);
        setHeaders(request, _headers);
        request.Method = "POST";

        var data = Encoding.ASCII.GetBytes(_postData);
        request.ContentLength = data.Length;

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(_postData);
            streamWriter.Flush();
            streamWriter.Close();
        }
        try
        {
            HttpWebResponse resp = await request.GetResponseAsync() as HttpWebResponse;
            return new WebResponse() { HttpStatusCode = resp.StatusCode, body = await responseStream(resp) };
        }
        catch (WebException ex)
        {
            return new WebResponse() { HttpStatusCode = ((HttpWebResponse)ex.Response).StatusCode, body = await responseStream((HttpWebResponse) ex.Response) };
        }
    }
    public async Task<WebResponse> get(string _url, Dictionary<string, string> _headers)
    {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            setHeaders(request, _headers);
            request.Method = "GET";
            try
            {
                HttpWebResponse resp = await request.GetResponseAsync() as HttpWebResponse;
                return new WebResponse() { HttpStatusCode = resp.StatusCode, body = await responseStream(resp) };
            }
            catch (WebException ex)
            {
                return new WebResponse() { HttpStatusCode = ((HttpWebResponse)ex.Response).StatusCode, body = await responseStream((HttpWebResponse)ex.Response) };
            }
    }
}