import 'package:flutter/material.dart';

class SemesterModel with ChangeNotifier {
  String? id;
  String? term;
  String? dateStart;
  String? dateStartFormat;
  String? dateEnd;
  String? dateEndFormat;
  int? status;
  String? dateStatus;

  SemesterModel(
      {this.id,
      this.term,
      this.dateStart,
      this.dateStartFormat,
      this.dateEnd,
      this.dateEndFormat,
      this.status,
      this.dateStatus});

  SemesterModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    term = json['Term'];
    dateStart = json['DateStart'];
    dateStartFormat = json['DateStartFormat'];
    dateEnd = json['DateEnd'];
    dateEndFormat = json['DateEndFormat'];
    status = json['Status'];
    dateStatus = json['DateStatus'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['Term'] = term;
    data['DateStart'] = dateStart;
    data['DateStartFormat'] = dateStartFormat;
    data['DateEnd'] = dateEnd;
    data['DateEndFormat'] = dateEndFormat;
    data['Status'] = status;
    data['DateStatus'] = dateStatus;
    return data;
  }

  static List<SemesterModel> semesterFromSnapshot(List semesterSnapshot) {
    return semesterSnapshot.map((data) {
      return SemesterModel.fromJson(data);
    }).toList();
  }
}
