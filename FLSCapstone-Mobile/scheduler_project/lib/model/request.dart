class RequestModel {
  String? id;
  String? title;
  String? description;
  String? dateCreate;
  String? dateCreateFormat;
  String? dateRespone;
  String? dateResponeFormat;
  String? lecturerId;
  String? lecturerName;
  String? departmentManagerId;
  String? departmentManagerName;
  String? subjectId;
  String? subjectName;
  String? semesterId;
  String? term;
  int? responseState;
  int? status;

  RequestModel(
      {this.id,
      this.title,
      this.description,
      this.dateCreate,
      this.dateCreateFormat,
      this.dateRespone,
      this.dateResponeFormat,
      this.lecturerId,
      this.lecturerName,
      this.departmentManagerId,
      this.departmentManagerName,
      this.subjectId,
      this.subjectName,
      this.semesterId,
      this.term,
      this.responseState,
      this.status});

  RequestModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    title = json['Title'];
    description = json['Description'];
    dateCreate = json['DateCreate'];
    dateCreateFormat = json['DateCreateFormat'];
    dateRespone = json['DateRespone'];
    dateResponeFormat = json['DateResponeFormat'];
    lecturerId = json['LecturerId'];
    lecturerName = json['LecturerName'];
    departmentManagerId = json['DepartmentManagerId'];
    departmentManagerName = json['DepartmentManagerName'];
    subjectId = json['SubjectId'];
    subjectName = json['SubjectName'];
    semesterId = json['SemesterId'];
    term = json['Term'];
    responseState = json['ResponseState'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['Title'] = title;
    data['Description'] = description;
    data['DateCreate'] = dateCreate;
    data['DateCreateFormat'] = dateCreateFormat;
    data['DateRespone'] = dateRespone;
    data['DateResponeFormat'] = dateResponeFormat;
    data['LecturerId'] = lecturerId;
    data['LecturerName'] = lecturerName;
    data['DepartmentManagerId'] = departmentManagerId;
    data['DepartmentManagerName'] = departmentManagerName;
    data['SubjectId'] = subjectId;
    data['SubjectName'] = subjectName;
    data['SemesterId'] = semesterId;
    data['Term'] = term;
    data['ResponseState'] = responseState;
    data['Status'] = status;
    return data;
  }

  static List<RequestModel> requestFromSnapshot(List requestSnapshot) {
    return requestSnapshot.map((data) {
      return RequestModel.fromJson(data);
    }).toList();
  }
}
