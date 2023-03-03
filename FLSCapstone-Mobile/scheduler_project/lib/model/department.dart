class DepartmentModel {
  String? id;
  String? departmentName;
  String? departmentGroupId;
  String? departmentGroupName;
  int? status;

  DepartmentModel(
      {this.id,
      this.departmentName,
      this.departmentGroupId,
      this.departmentGroupName,
      this.status});

  DepartmentModel.fromJson(Map<String, dynamic> json) {
    id = json['Id'];
    departmentName = json['DepartmentName'];
    departmentGroupId = json['DepartmentGroupId'];
    departmentGroupName = json['DepartmentGroupName'];
    status = json['Status'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['Id'] = id;
    data['DepartmentName'] = departmentName;
    data['DepartmentGroupId'] = departmentGroupId;
    data['DepartmentGroupName'] = departmentGroupName;
    data['Status'] = status;
    return data;
  }

  static List<DepartmentModel> departmentFromSnapshot(List departmentSnapshot) {
    return departmentSnapshot.map((data) {
      return DepartmentModel.fromJson(data);
    }).toList();
  }
}
