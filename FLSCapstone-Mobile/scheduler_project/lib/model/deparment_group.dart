class DepartmentGroupModel {
  String? id;
  String? departmentGroupName;
  int? status;
  List<String>? departmentIds;

  DepartmentGroupModel(
      {this.id, this.departmentGroupName, this.status, this.departmentIds});

  DepartmentGroupModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    departmentGroupName = json['DepartmentGroupName'];
    status = json['Status'];
    departmentIds = json['DepartmentIds'].cast<String>();
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['DepartmentGroupName'] = departmentGroupName;
    data['Status'] = status;
    data['DepartmentIds'] = departmentIds;
    return data;
  }

  static List<DepartmentGroupModel> departmentGroupFromSnapshot(
      List departmentGroupSnapshot) {
    return departmentGroupSnapshot.map((data) {
      return DepartmentGroupModel.fromJson(data);
    }).toList();
  }
}
