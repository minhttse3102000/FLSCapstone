class ScheduleModel {
  String? id;
  int? isPublic;
  String? semesterId;
  String? term;
  int? status;

  ScheduleModel(
      {this.id, this.isPublic, this.semesterId, this.term, this.status});

  ScheduleModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    isPublic = json['IsPublic'];
    semesterId = json['SemesterId'];
    term = json['Term'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['IsPublic'] = isPublic;
    data['SemesterId'] = semesterId;
    data['Term'] = term;
    data['Status'] = status;
    return data;
  }

  static List<ScheduleModel> schedulerFromSnapshot(List schedulerSnapshot) {
    return schedulerSnapshot.map((data) {
      return ScheduleModel.fromJson(data);
    }).toList();
  }
}
