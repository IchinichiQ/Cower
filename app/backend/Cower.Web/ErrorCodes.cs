namespace Cower.Web;

public static class ErrorCodes
{
    public const string INTERNAL_SERVER_ERROR = "internal_server_error";
    public const string UNAUTHORIZED = "unauthorized";
    public const string FORBIDDEN = "forbidden";
    public const string EMAIL_ALREADY_TAKEN = "email_already_taken";
    public const string INVALID_REQUEST_DATA = "invalid_request_data";
    public const string INVALID_CREDENTIALS = "invalid_credentials";
    public const string NOT_FOUND = "not_found";
    public const string FLOOR_NUMBER_EXIST_IN_COWORKING = "floor_number_exist_in_coworking";
    public const string COWORKING_DOESNT_EXIST = "coworking_doesnt_exist";
    public const string INVALID_FILE_TYPE = "invalid_file_type";
    public const string ENTITY_USED_BY_OTHERS = "entity_used_by_others";
    public const string IMAGE_DOESNT_EXIST = "image_doesnt_exist";
    public const string WRONG_IMAGE_TYPE = "wrong_image_type";
    public const string SEAT_NUMBER_EXIST_ON_FLOOR = "seat_number_exist_on_floor";
    public const string FLOOR_DOESNT_EXIST = "floor_doesnt_exist";
    public const string PASSWORD_RESET_TOKEN_DOESNT_EXIST = "password_reset_token_doesnt_exist";
    public const string PASSWORD_RESET_TOKEN_EXPIRED = "password_reset_token_expired";
}