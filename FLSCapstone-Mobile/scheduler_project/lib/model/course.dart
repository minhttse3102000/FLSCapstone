class CourseModel {
  String? id;
  String? subjectId;
  String? subjectName;
  String? semesterId;
  String? term;
  String? description;
  int? slotAmount;
  int? status;

  CourseModel(
      {this.id,
      this.subjectId,
      this.subjectName,
      this.semesterId,
      this.term,
      this.description,
      this.slotAmount,
      this.status});

  CourseModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    subjectId = json['SubjectId'];
    subjectName = json['SubjectName'];
    semesterId = json['SemesterId'];
    term = json['Term'];
    description = json['Description'];
    slotAmount = json['SlotAmount'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['SubjectId'] = subjectId;
    data['SubjectName'] = subjectName;
    data['SemesterId'] = semesterId;
    data['Term'] = term;
    data['Description'] = description;
    data['SlotAmount'] = slotAmount;
    data['Status'] = status;
    return data;
  }
}
