class CourseAssignModel {
  String? id;
  String? lecturerId;
  String? lecturerName;
  String? courseId;
  String? slotTypeId;
  String? scheduleId;
  int? isAssign;
  int? status;

  CourseAssignModel(
      {this.id,
      this.lecturerId,
      this.lecturerName,
      this.courseId,
      this.slotTypeId,
      this.scheduleId,
      this.isAssign,
      this.status});

  CourseAssignModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    lecturerId = json['LecturerId'];
    lecturerName = json['LecturerName'];
    courseId = json['CourseId'];
    slotTypeId = json['SlotTypeId'];
    scheduleId = json['ScheduleId'];
    isAssign = json['isAssign'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['LecturerId'] = lecturerId;
    data['LecturerName'] = lecturerName;
    data['CourseId'] = courseId;
    data['SlotTypeId'] = slotTypeId;
    data['ScheduleId'] = scheduleId;
    data['isAssign'] = isAssign;
    data['Status'] = status;
    return data;
  }

  static List<CourseAssignModel> courseAssignFromSnapshot(
      List courseAssignSnapshot) {
    return courseAssignSnapshot.map((data) {
      return CourseAssignModel.fromJson(data);
    }).toList();
  }
}
