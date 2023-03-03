class RoleModel {
  String? id;
  String? roleName;
  int? status;

  RoleModel({this.id, this.roleName, this.status});

  RoleModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    roleName = json['RoleName'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['RoleName'] = roleName;
    data['Status'] = status;
    return data;
  }
}
