import 'package:flutter/material.dart';

class UserModel with ChangeNotifier {
  String? id;
  String? name;
  String? email;
  String? dob;
  int? gender;
  String? idcard;
  String? address;
  String? phone;
  int? priorityLecturer;
  int? isFullTime;
  String? departmentId;
  String? departmentName;
  List<String>? roleIDs;
  int? status;
  String? dateOfBirthFormatted;

  UserModel(
      {this.id,
      this.name,
      this.email,
      this.dob,
      this.gender,
      this.idcard,
      this.address,
      this.phone,
      this.priorityLecturer,
      this.isFullTime,
      this.departmentId,
      this.departmentName,
      this.roleIDs,
      this.status,
      this.dateOfBirthFormatted});

  UserModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    name = json['Name'];
    email = json['Email'];
    dob = json['Dob'];
    gender = json['Gender'];
    idcard = json['Idcard'];
    address = json['Address'];
    phone = json['Phone'];
    priorityLecturer = json['PriorityLecturer'];
    isFullTime = json['IsFullTime'];
    departmentId = json['DepartmentId'];
    departmentName = json['DepartmentName'];
    roleIDs = json['RoleIDs'].cast<String>();
    status = json['Status'];
    dateOfBirthFormatted = json['DateOfBirthFormatted'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['Name'] = name;
    data['Email'] = email;
    data['Dob'] = dob;
    data['Gender'] = gender;
    data['Idcard'] = idcard;
    data['Address'] = address;
    data['Phone'] = phone;
    data['PriorityLecturer'] = priorityLecturer;
    data['IsFullTime'] = isFullTime;
    data['DepartmentId'] = departmentId;
    data['DepartmentName'] = departmentName;
    data['RoleIDs'] = roleIDs;
    data['Status'] = status;
    data['DateOfBirthFormatted'] = dateOfBirthFormatted;
    return data;
  }
}
