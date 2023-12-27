namespace PetShop.DTOs
{
    public class AppointmentDto
    {
        public int Dog_item_id { get; set; }
        public string User_id { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Description { get; set; }
        public string Phone_number { get; set; }
        public string User_name { get; set; }
        public string Service { get; set; }
        public string? Result { get; set; }
        public string? Status { get; set; }
    }

    public class Appointment1Dto
    {
        public string Result { get; set; }
        public string Status { get; set; }
    }

    public class Appointment2Dto
    {
        public string Result { get; set; }
    }
}
