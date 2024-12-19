

    public class NotificationsUser
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public long? LocationId { get; set; }
        public long? CompanyId { get; set; }
        public string UserEmail { get; set; }
    
        public bool? IsAlertSent { get; set; }

        public string UserLink { get; set; }
        public string TableName { get; set; } 
        public long? TableId { get; set; }
        public string DeveloperDescriptionState { get; set; }//notes sent to developer

        public DateTime CreatedDate { get; set; }



        public long NotificationActionId { get; set; }
        public string NotificationEventId { get; set; }
       
        public short? StatusId { get; set; }
   
   
        public bool? IsEmailSent { get; set; }
       

     

    }