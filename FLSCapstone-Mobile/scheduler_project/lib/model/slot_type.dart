class SlotTypeModel {
  String? id;
  String? timeStart;
  String? timeEnd;
  int? slotNumber;
  int? dateOfWeek;
  String? duration;
  String? semesterId;
  String? term;
  String? convertDateOfWeek;
  int? status;

  SlotTypeModel(
      {this.id,
      this.timeStart,
      this.timeEnd,
      this.slotNumber,
      this.dateOfWeek,
      this.duration,
      this.semesterId,
      this.term,
      this.convertDateOfWeek,
      this.status});

  SlotTypeModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    timeStart = json['TimeStart'];
    timeEnd = json['TimeEnd'];
    slotNumber = json['SlotNumber'];
    dateOfWeek = json['DateOfWeek'];
    duration = json['Duration'];
    semesterId = json['SemesterId'];
    term = json['Term'];
    convertDateOfWeek = json['ConvertDateOfWeek'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['TimeStart'] = timeStart;
    data['TimeEnd'] = timeEnd;
    data['SlotNumber'] = slotNumber;
    data['DateOfWeek'] = dateOfWeek;
    data['Duration'] = duration;
    data['SemesterId'] = semesterId;
    data['Term'] = term;
    data['ConvertDateOfWeek'] = convertDateOfWeek;
    data['Status'] = status;
    return data;
  }

  static List<SlotTypeModel> slotTypeFromSnapshot(List slotTypeSnapshot) {
    return slotTypeSnapshot.map((data) {
      return SlotTypeModel.fromJson(data);
    }).toList();
  }
}
