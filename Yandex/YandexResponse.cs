namespace YandexDiskFileUploader.Yandex
{
    public class YandexResponse
    {
        public string operation_id { get; set; }
        public string href { get; set; }
        public string method { get; set; }
        public bool templated { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string error { get; set; }
    }
}