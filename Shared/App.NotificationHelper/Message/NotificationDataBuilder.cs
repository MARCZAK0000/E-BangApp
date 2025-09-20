using App.NotificationHelper.Enums;
using App.NotificationHelper.Exceptions;

namespace App.NotificationHelper.Message
{
    public class NotificationDataBuilder
    {
        private string _title = string.Empty;
        private string _text = string.Empty;
        private AccountNotificationData? _senderData;
        private List<AccountNotificationData>? _reciverDataList;
        private AccountNotificationData? _receiverData;
        private ENotificationReciver _eNotificationRecivers;
        private ENotificationType _eNotificationType;
        private DateTime _notificationDate;

        /// <summary>
        /// Sets the notification title and returns the current <see cref="NotificationDataBuilder"/> instance.
        /// </summary>
        /// <param name="title">The title to be displayed in the notification. Cannot be null or empty.</param>
        /// <returns>The current <see cref="NotificationDataBuilder"/> instance, allowing for method chaining.</returns>
        public NotificationDataBuilder NotificationTitle(string title)
        {
            _title = title;
            return this;
        }
        /// <summary>
        /// Sets the notification text to be displayed.
        /// </summary>
        /// <param name="text">The text content of the notification. Cannot be null.</param>
        /// <returns>The current <see cref="NotificationDataBuilder"/> instance, allowing for method chaining.</returns>
        public NotificationDataBuilder NotificationText(string text)
        {
            _text = text;
            return this;
        }
        /// <summary>
        /// Sets the sender data for the notification.
        /// </summary>
        /// <param name="senderData">The sender data to associate with the notification. Cannot be <see langword="null"/>.</param>
        /// <returns>The current <see cref="NotificationDataBuilder"/> instance, allowing for method chaining.</returns>
        /// <exception cref="InvalidNotificationData">Thrown if <paramref name="senderData"/> is <see langword="null"/>.</exception>
        public NotificationDataBuilder SenderData(AccountNotificationData senderData)
        {
            if(senderData is null)
                throw new InvalidNotificationData("Sender data is null.");
            _senderData = senderData;
            return this;
        }
        /// <summary>
        /// Sets the receiver data for the notification.
        /// </summary>
        /// <param name="receiverData">The receiver data to associate with the notification. Cannot be <see langword="null"/>.</param>
        /// <returns>The current <see cref="NotificationDataBuilder"/> instance, allowing for method chaining.</returns>
        /// <exception cref="InvalidNotificationData">Thrown if <paramref name="receiverData"/> is <see langword="null"/>.</exception>
        public NotificationDataBuilder ReceiverData(AccountNotificationData receiverData)
        {
            if(receiverData is null)
                throw new InvalidNotificationData("Receiver data is null.");
            _receiverData = receiverData;
            _reciverDataList = null;
            return this;
        }
        /// <summary>
        /// Sets the list of receiver notification data for the notification being built.
        /// </summary>
        /// <param name="reciverDataList">A list of <see cref="AccountNotificationData"/> objects representing the notification data for the
        /// receivers. The list must not be null or empty.</param>
        /// <returns>The current instance of <see cref="NotificationDataBuilder"/> to allow method chaining.</returns>
        /// <exception cref="InvalidNotificationData">Thrown if <paramref name="reciverDataList"/> is null or empty.</exception>
        public NotificationDataBuilder ReceiverDataList(List<AccountNotificationData> reciverDataList)
        {
            if(reciverDataList is null || !reciverDataList.Any())
                throw new InvalidNotificationData("Receiver data list is null or empty.");
            _reciverDataList = reciverDataList;
            _receiverData = null;
            return this;
        }
        /// <summary>
        /// Sets the notification recipients for the notification being built.
        /// </summary>
        /// <param name="eNotificationRecivers">An enumeration value specifying the recipients of the notification.</param>
        /// <returns>The current <see cref="NotificationDataBuilder"/> instance, allowing for method chaining.</returns>
        public NotificationDataBuilder NotificationRecivers(ENotificationReciver eNotificationRecivers)
        {
            _eNotificationRecivers = eNotificationRecivers;
            return this;
        }
        /// <summary>
        /// Sets the notification type for the builder.
        /// </summary>
        /// <param name="eNotificationType">The type of notification to set. This value determines the notification category to be used.</param>
        /// <returns>The current instance of <see cref="NotificationDataBuilder"/> to allow method chaining.</returns>
        public NotificationDataBuilder NotificationType(ENotificationType eNotificationType)
        {
            _eNotificationType = eNotificationType;
            return this;
        }
        /// <summary>
        /// Builds and returns a <see cref="NotificationDataMessage"/> instance using the configured properties.
        /// </summary>
        /// <remarks>This method finalizes the notification data by setting the notification date to the
        /// current UTC time. It ensures that the title and text are not null, empty, or whitespace before creating the
        /// message.</remarks>
        /// <returns>A <see cref="NotificationDataMessage"/> instance containing the configured notification details.</returns>
        /// <exception cref="InvalidNotificationData">Thrown if the notification title or text is not set (null, empty, or consists only of whitespace).</exception>
        public NotificationDataMessage Build()
        {
            _notificationDate = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(_title) || string.IsNullOrWhiteSpace(_text))
                throw new InvalidNotificationData("Notification title is not set.");
            return new NotificationDataMessage(_title, _text, _senderData, _receiverData, 
                _reciverDataList,_eNotificationRecivers, _eNotificationType, _notificationDate);
        }
    }
}
