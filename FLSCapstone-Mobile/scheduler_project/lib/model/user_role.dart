class UserRoleModel {
  String? id;
  String? userId;
  String? userName;
  String? roleId;
  String? roleName;
  int? status;

  UserRoleModel(
      {this.id,
      this.userId,
      this.userName,
      this.roleId,
      this.roleName,
      this.status});

  UserRoleModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    userId = json['UserId'];
    userName = json['UserName'];
    roleId = json['RoleId'];
    roleName = json['RoleName'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['UserId'] = userId;
    data['UserName'] = userName;
    data['RoleId'] = roleId;
    data['RoleName'] = roleName;
    data['Status'] = status;
    return data;
  }
}
