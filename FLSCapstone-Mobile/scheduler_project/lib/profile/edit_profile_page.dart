// ignore_for_file: prefer_const_literals_to_create_immutables, unused_local_variable, prefer_interpolation_to_compose_strings, use_build_context_synchronously, prefer_final_fields, no_leading_underscores_for_local_identifiers

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import '../consts/api_const.dart';
import '../model/user.dart';
import 'componets/showMessage.dart';

class EditProfilePage extends StatefulWidget {
  final UserModel userModel;
  const EditProfilePage({
    Key? key,
    required this.userModel,
  }) : super(key: key);

  @override
  State<EditProfilePage> createState() => _EditProfilePageState();
}

class _EditProfilePageState extends State<EditProfilePage> {
  final formKey = GlobalKey<FormState>();
  final TextEditingController _controllerFullName = TextEditingController();
  final TextEditingController _controllerAddress = TextEditingController();
  TextEditingController _controllerDateOfBirth = TextEditingController();
  final TextEditingController _controllerPhone = TextEditingController();
  final TextEditingController _controllerCardId = TextEditingController();
  DateTime _selectedDate = DateTime.now();
  @override
  void initState() {
    _controllerFullName.text = widget.userModel.name.toString();
    _controllerAddress.text = widget.userModel.address.toString();
    _controllerDateOfBirth.text = DateFormat.yMMMMd().format(
        DateTime.parse(widget.userModel.dateOfBirthFormatted.toString()));
    _controllerPhone.text = widget.userModel.phone.toString();
    _controllerCardId.text = widget.userModel.idcard.toString();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              color: Colors.green[700],
              padding: EdgeInsets.only(left: 20, right: 20),
              height: size.height * 0.20,
              width: double.infinity,
              child: Column(
                children: [
                  SizedBox(
                    height: 30,
                  ),
                  Row(
                    children: [
                      GestureDetector(
                        onTap: () {
                          Navigator.pop(context);
                        },
                        child: Icon(
                          Icons.arrow_back_outlined,
                          size: 30,
                          color: Colors.white,
                        ),
                      ),
                      SizedBox(width: 10),
                      Text(
                        'Edit Profile',
                        style: TextStyle(fontSize: 20, color: Colors.white),
                      ),
                    ],
                  ),
                  //SizedBox(height: 10),
                  CircleAvatar(
                    backgroundColor: Colors.white,
                    radius: 45,
                    backgroundImage: AssetImage('assets/images/profile.png'),
                  ),
                ],
              ),
            ),
            SizedBox(height: 10),
            Container(
              padding: EdgeInsets.only(left: 20, right: 20),
              child: Form(
                key: formKey,
                child: Column(
                  //crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(bottom: 8),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Full Name',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                          SizedBox(height: 8),
                          TextFormField(
                            validator: (value) {
                              if (value!.isEmpty) {
                                return 'Fill the blanks';
                              } else {
                                return null;
                              }
                            },
                            style: TextStyle(height: 1.2),
                            controller: _controllerFullName,
                            decoration: InputDecoration(
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                          )
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(bottom: 8),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Date Of Birth',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                          SizedBox(height: 8),
                          TextFormField(
                            validator: (value) {
                              if (value!.isEmpty) {
                                return 'Fill the blanks';
                              }
                              return null;
                            },
                            style: TextStyle(height: 1.2),
                            controller: _controllerDateOfBirth,
                            decoration: InputDecoration(
                              suffixIcon: IconButton(
                                onPressed: () {
                                  print('Hi there');
                                  _getDateFromUser();
                                },
                                icon: Icon(Icons.calendar_today_outlined,
                                    color: Colors.grey),
                              ),
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(bottom: 8),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Address',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                          SizedBox(height: 8),
                          TextFormField(
                            validator: (value) {
                              if (value!.isEmpty) {
                                return 'Fill the blanks';
                              }
                              return null;
                            },
                            style: TextStyle(height: 1.2),
                            controller: _controllerAddress,
                            decoration: InputDecoration(
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                          )
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(bottom: 8),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Phone',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                          SizedBox(height: 8),
                          TextFormField(
                            validator: (value) {
                              if (value!.length < 10) {
                                return 'Length must be 10';
                              }
                              return null;
                            },
                            style: TextStyle(height: 1.2),
                            controller: _controllerPhone,
                            decoration: InputDecoration(
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                          )
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(bottom: 8),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            'Card Id',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                          SizedBox(height: 8),
                          TextFormField(
                            validator: (value) {
                              if (value!.length < 12) {
                                return 'Length mus be 12';
                              }
                              return null;
                            },
                            style: TextStyle(height: 1.2),
                            controller: _controllerCardId,
                            decoration: InputDecoration(
                              border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(12),
                              ),
                            ),
                          )
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
            SizedBox(height: 3),
            Container(
              alignment: Alignment.center,
              child: ElevatedButton(
                style: ElevatedButton.styleFrom(
                    primary: Colors.green[500], minimumSize: Size(100, 50)),
                onPressed: () {
                  if (formKey.currentState!.validate()) {}
                  editUser();
                },
                child: Text(
                  'Save',
                  style: TextStyle(fontSize: 20, color: Colors.white),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  _getDateFromUser() async {
    DateTime? _pickerDate = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(2015),
      lastDate: DateTime(2024),
    );

    if (_pickerDate != null) {
      setState(() {
        _selectedDate = _pickerDate;
        _controllerDateOfBirth.text = DateFormat.yMMMMd().format(_selectedDate);
      });
    } else {
      print("It's null or something is wrong");
    }
  }

  editUser() async {
    var fullName = _controllerFullName.text;
    var email = widget.userModel.email.toString();
    var gender = widget.userModel.gender.toString();
    var idCard = _controllerCardId.text;
    var address = _controllerAddress.text;
    var phone = _controllerPhone.text;
    var dob = _controllerDateOfBirth.text;
    var priority = widget.userModel.priorityLecturer.toString();
    var isFulltime = widget.userModel.isFullTime.toString();
    var departId = widget.userModel.departmentId.toString();

    var url = BASE_API + "User/${widget.userModel.id.toString()}";
    print(url);
    var bodyData = json.encode({
      "Name": fullName,
      "Email": email,
      "Dob": dob,
      "Gender": gender,
      "Idcard": idCard,
      "Address": address,
      "Phone": phone,
      "PriorityLecturer": priority,
      "IsFullTime": isFulltime,
      "DepartmentId": departId
    });

    var response = await http.put(Uri.parse(url),
        headers: {
          "Content-Type": "application/json-patch+json",
          "Accept": "application/json"
        },
        body: bodyData);
    if (response.statusCode == 200) {
      var messageSuccess = 'Edit success';
      showMessage(context, messageSuccess, widget.userModel);
    } else {
      var messageError = "Can not update this user!!";
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(
        content: Text(messageError),
        duration: Duration(seconds: 10),
      ));
    }
  }
}
