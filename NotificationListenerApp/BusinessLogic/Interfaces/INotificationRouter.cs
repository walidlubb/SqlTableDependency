public interface INotificationRouter
{

  

    void Route(Notification notification);
   

     bool ValidateRouting() => true;
}