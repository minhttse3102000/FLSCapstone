import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:scheduler_project/model/course_asign.dart';
import 'package:scheduler_project/model/department.dart';
import 'package:scheduler_project/model/scheduler.dart';
import 'package:scheduler_project/model/semester.dart';
import 'package:scheduler_project/model/slot_type.dart';
import 'package:scheduler_project/model/subject.dart';
import 'package:scheduler_project/model/user.dart';

import '../consts/api_const.dart';
import '../model/request.dart';
import '../model/token.dart';

class APIHandler {
  static Future<List<dynamic>> getData({required String target}) async {
    //http://20.214.249.72/api/Semester?pageIndex=1&pageSize=1000
    var uri = Uri.http(BASE_URL, '/api/$target', PARAMS);
    print('Uri : $uri');
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return tempList;
  }

  static Future<List<SemesterModel>> getAllSemester() async {
    List temp = await getData(target: 'Semester');

    return SemesterModel.semesterFromSnapshot(temp);
  }

  static Future<UserModel> getUserByEmail({required String email}) async {
    Map<String, String> params = {
      'Email': email,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/User', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);

    return UserModel.fromJson(data[0]);
  }

  static Future<DepartmentModel> getDepartmentById(
      {required String departId}) async {
    Map<String, String> params = {
      'Id': departId,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Department', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    return DepartmentModel.fromJson(data[0]);
  }

  static Future<List<DepartmentModel>> getAllDepartmentByDepartGroupId(
      {required String departmentGroupId}) async {
    Map<String, String> params = {
      'DepartmentGroupId': departmentGroupId,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Department', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return DepartmentModel.departmentFromSnapshot(tempList);
  }

  static Future<List<SubjectModel>> getAllSubjectByDepartId(
      {required String departmentId}) async {
    Map<String, String> params = {
      'DepartmentId': departmentId,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Subject', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return SubjectModel.departmentFromSnapshot(tempList);
  }

  static Future<ScheduleModel> getSchedulerBySemesID(
      {required String semesterId}) async {
    Map<String, String> params = {
      'SemesterId': semesterId,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Schedule', params);
    print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    if (data.length > 0) {
      return ScheduleModel.fromJson(data[0]);
    } else {
      ScheduleModel scheduleModel = ScheduleModel();
      return scheduleModel;
    }
  }

  static Future<List<CourseAssignModel>> getAllCourseAssignByLeSche(
      {required String lecturerId, required String schedulerId}) async {
    Map<String, String> params = {
      'LecturerId': lecturerId,
      'ScheduleId': schedulerId,
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/CourseAssign', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return CourseAssignModel.courseAssignFromSnapshot(tempList);
  }

  static Future<List<SlotTypeModel>> getAllSlotType() async {
    List temp = await getData(target: 'SlotType');

    return SlotTypeModel.slotTypeFromSnapshot(temp);
  }

  static Future<List<SlotTypeModel>> getAllSlotTypeAsc() async {
    Map<String, String> params = {
      'sortBy': 'SlotNumber',
      'order': 'Asc',
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http('20.214.249.72', '/api/SlotType', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return SlotTypeModel.slotTypeFromSnapshot(tempList);
  }

  static Future<UserModel> getManagerByDepartId(
      {required String departId}) async {
    Map<String, String> params = {
      'DepartmentId': departId,
      'RoleIDs': 'DMA',
      'pageIndex': '1',
      'pageSize': '1',
    };
    var uri = Uri.http(BASE_URL, '/api/User', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    if (data.length > 0) {
      return UserModel.fromJson(data[0]);
    } else {
      UserModel userModel = UserModel();
      return userModel;
    }
  }

  static Future<List<SemesterModel>> getAllSemesterDec() async {
    Map<String, String> params = {
      'sortBy': 'DateEnd',
      'order': 'Des',
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Semester', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return SemesterModel.semesterFromSnapshot(tempList);
  }

  static Future<List<RequestModel>> getAllRequestByLecturer(
      {required String lecturerId, required String semesterId}) async {
    Map<String, String> params = {
      'LecturerId': lecturerId,
      'SemesterId': semesterId,
      'sortBy': 'DateCreate',
      'order': 'Des',
      'pageIndex': '1',
      'pageSize': '1000',
    };
    var uri = Uri.http(BASE_URL, '/api/Request', params);
    //print(uri);
    var response = await http.get(uri);
    var data = jsonDecode(response.body);
    //print(data);
    List tempList = [];
    for (var v in data) {
      //print("V $v \n\n");
      tempList.add(v);
    }

    return RequestModel.requestFromSnapshot(tempList);
  }

  static Future<TokenModel> getTokenByEmail({required String email}) async {
    Map<String, String> params = {
      'email': email,
    };
    var uri = Uri.http('20.214.249.72', '/api/Token/Login', params);
    print(uri);
    var response = await http.post(uri, headers: {
      "Content-Type": "application/json; charset=UTF-8",
    });
    var data = jsonDecode(response.body);
    print(data);

    return TokenModel.fromJson(data);
  }

  static Future<UserModel> getUserAuthenByEmail(
      {required String email, required String token}) async {
    Map<String, String> params = {
      'Email': email,
      'pageIndex': '1',
      'pageSize': '1',
    };
    var uri = Uri.http('20.214.249.72', '/api/UserAuthen', params);
    print(uri);
    var response = await http.get(uri, headers: {
      "Content-Type": "application/json; charset=UTF-8",
      "Authorization": "Bearer $token"
    });
    var data = jsonDecode(response.body);
    print(data);

    return UserModel.fromJson(data[0]);
  }
}
