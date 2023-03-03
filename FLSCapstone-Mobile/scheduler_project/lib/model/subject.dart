import 'package:flutter/material.dart';

class SubjectModel with ChangeNotifier {
  String? id;
  String? subjectName;
  String? description;
  String? departmentId;
  String? departmentName;
  int? status;

  SubjectModel(
      {this.id,
      this.subjectName,
      this.description,
      this.departmentId,
      this.departmentName,
      this.status});

  SubjectModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    subjectName = json['SubjectName'];
    description = json['Description'];
    departmentId = json['DepartmentId'];
    departmentName = json['DepartmentName'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['SubjectName'] = subjectName;
    data['Description'] = description;
    data['DepartmentId'] = departmentId;
    data['DepartmentName'] = departmentName;
    data['Status'] = status;
    return data;
  }

  static List<SubjectModel> departmentFromSnapshot(List subjectSnapshot) {
    return subjectSnapshot.map((data) {
      return SubjectModel.fromJson(data);
    }).toList();
  }
}
