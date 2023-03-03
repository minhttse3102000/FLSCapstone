class TokenModel {
  bool? success;
  String? message;
  String? accessToken;
  String? refreshToken;

  TokenModel({this.success, this.message, this.accessToken, this.refreshToken});

  TokenModel.fromJson(Map<String, dynamic> json) {
    success = json['success'];
    message = json['message'];
    accessToken = json['access_token'];
    refreshToken = json['refresh_token'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['success'] = this.success;
    data['message'] = this.message;
    data['access_token'] = this.accessToken;
    data['refresh_token'] = this.refreshToken;
    return data;
  }
}
