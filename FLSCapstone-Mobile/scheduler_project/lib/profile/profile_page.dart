// ignore_for_file: prefer_const_literals_to_create_immutables

import 'package:flutter/material.dart';
import 'package:scheduler_project/profile/edit_profile_page.dart';
import 'package:scheduler_project/services/api_handler.dart';

import '../model/user.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({
    Key? key,
    required this.userModel,
  }) : super(key: key);
  final UserModel userModel;

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  UserModel user = UserModel();
  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getUser();
  }

  Future<void> getUser() async {
    user = await APIHandler.getUserByEmail(
        email: widget.userModel.email.toString());
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Scaffold(
      body: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            color: Colors.green[700],
            padding: EdgeInsets.only(left: 20, right: 20, top: 10),
            height: size.height * 0.255,
            width: double.infinity,
            child: Column(
              children: [
                SizedBox(
                  height: 25,
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
                    SizedBox(width: 15),
                    Text(
                      'Profile',
                      style: TextStyle(fontSize: 23, color: Colors.white),
                    ),
                  ],
                ),
                Column(
                  children: [
                    CircleAvatar(
                      backgroundColor: Colors.white,
                      radius: 45,
                      backgroundImage: AssetImage('assets/images/profile.png'),
                    ),
                    SizedBox(height: 10),
                    Text(
                      user.name.toString(),
                      style: TextStyle(fontSize: 22, color: Colors.white),
                    ),
                  ],
                ),
              ],
            ),
          ),
          Container(
            color: Colors.white,
            child: Column(
              children: [
                _builderTextField('Department', user.departmentName.toString()),
                _builderTextField(
                    'Date Of Birth', user.dateOfBirthFormatted.toString()),
                _builderTextField('Email', user.email.toString()),
                _builderTextField('Phone Number', user.phone.toString()),
                _builderTextField('Card Id', user.idcard.toString()),
                _builderTextField(
                    'Lecturer Type',
                    user.isFullTime.toString() == '1'
                        ? 'Full-time'
                        : 'Part-time'),
              ],
            ),
          ),
          SizedBox(height: 10),
          Container(
            //margin: EdgeInsets.only(left: 10),
            alignment: Alignment.center,
            child: ElevatedButton(
              style: ElevatedButton.styleFrom(
                primary: Colors.green[500],
              ),
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => EditProfilePage(userModel: user),
                  ),
                );
              },
              child: Text(
                'Edit Profile',
                style: TextStyle(fontSize: 20, color: Colors.white),
              ),
            ),
          )
        ],
      ),
    );
  }

  _builderTextField(String labelText, String placeholder) {
    return Padding(
      padding: const EdgeInsets.only(left: 20, right: 20, top: 20),
      child: TextField(
        obscureText: false,
        decoration: InputDecoration(
          contentPadding: EdgeInsets.only(bottom: 5),
          labelText: labelText,
          labelStyle: TextStyle(
              fontSize: 30, fontWeight: FontWeight.bold, color: Colors.black),
          floatingLabelBehavior: FloatingLabelBehavior.always,
          hintText: placeholder,
          hintStyle: TextStyle(
              fontSize: 20, fontWeight: FontWeight.bold, color: Colors.grey),
        ),
      ),
    );
  }
}
