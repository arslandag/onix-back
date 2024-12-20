namespace Onix.SharedKernel;

public static class Constants
{
    //max length
    public const int CODE_MAX_LENGTH = 1500;
    public const int NAME_MAX_LENGTH = 50;
    public const int PHONE_MAX_LENGTH = 25;
    public const int SOCIAL_MAX_LENGTH = 20;
    public const int EMAIL_MAX_LENGTH = 256;
    public const int URL_MAX_LENGTH = 50;
    public const int LINK_MAX_LENGTH = 200;
    public const int PATH_MAX_LENGTH = 100;
    public const int TITLE_MAX_LENGTH = 70;
    public const int DESCRIPTION_MAX_LENGTH = 800;
    public const int QUESTION_MAX_LENGTH = 150;
    public const int ANSWER_MAX_LENGTH = 800;
    public const int ADDRESS_MAX_LENGTH = 50;
    public const int INDEX_MAX_LENGHT = 6;
    public const int WEEKDAY_MAX_LENGTH = 15;
    public const int TIME_MAX_LENGTH = 5;
    public const int SHARE_MAX_LENGTH = 50;
    public const int JSON_MAX_LENGTH = 500;
    public const int PRICE_MAX_LENGTH = 50;
    
    //min length
    public const int NAME_MIN_LENGTH = 2;
    public const int URL_MIN_LENGTH = 2;
    
    //regax
    public const string URL_REGEX = "^[a-z]+(-[a-z]+)?$";
    public const string ID_REGEX = "^([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})$";

    //max count
    public const int MAX_BLOCK_COUNT = 10;
    public const int MAX_FAQ_COUNT = 15;
    public const int MAX_SOCIAL_COUNT = 10;
    public const int MAX_PRODUCT_COUNT = 25;
    public const int MAX_SERVICE_COUNT = 10;
    public const int MAX_LOCATION_COUNT = 10;
    public const int MAX_CATEGORY_COUNT = 7;
    public const int MAX_SCHEDULE_COUNT = 7;
    public const int MAX_PHOTO_COUNT = 5;
    
    //min count
    public const int MIN_COUNT = 0;
}
