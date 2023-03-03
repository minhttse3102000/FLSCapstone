// ignore_for_file: use_build_context_synchronously, sized_box_for_whitespace, unnecessary_null_comparison

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:scheduler_project/home/home_page.dart';
import 'package:scheduler_project/model/token.dart';
import 'package:scheduler_project/model/user.dart';

import '../consts/api_const.dart';
import '../google/google_sigin_api.dart';
import '../services/api_handler.dart';
import 'package:http/http.dart' as http;

import 'login_message.dart';

class Login extends StatefulWidget {
  static String routeName = 'login';

  @override
  State<Login> createState() => _LoginState();
}

class _LoginState extends State<Login> {
  UserModel userModel = UserModel();
  TokenModel tokenModel = TokenModel();
  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;

    Future signIn() async {
      final user = await GoogleSignInApi.login();
      userModel = await APIHandler.getUserByEmail(email: user!.email);
      setState(() {});
      if (userModel == null) {
        ScaffoldMessenger.of(context)
            .showSnackBar(SnackBar(content: Text('Sign In Failed!')));
      } else {
        for (var role in userModel.roleIDs!) {
          if (role == 'LC') {
            Navigator.of(context).pushReplacement(MaterialPageRoute(
              builder: (context) => HomePage(userModel: userModel),
            ));
          } else {
            ScaffoldMessenger.of(context)
                .showSnackBar(SnackBar(content: Text('Sign In Failed!')));
          }
        }
      }
    }

    // Future signIn() async {
    //   final user = await GoogleSignInApi.login();

    //   tokenModel = await APIHandler.getTokenByEmail(email: user!.email);

    //   userModel = await APIHandler.getUserAuthenByEmail(
    //       email: user.email, token: tokenModel.accessToken.toString());
    //   setState(() {});
    //   if (userModel == null) {
    //     ScaffoldMessenger.of(context)
    //         .showSnackBar(SnackBar(content: Text('Sign In Failed!')));
    //   } else {
    //     for (var role in userModel.roleIDs!) {
    //       if (role == 'LC') {
    //         Navigator.of(context).pushReplacement(MaterialPageRoute(
    //           builder: (context) => HomePage(userModel: userModel),
    //         ));
    //       } else {
    //         ScaffoldMessenger.of(context)
    //             .showSnackBar(SnackBar(content: Text('Sign In Failed!')));
    //       }
    //     }
    //   }
    // }

    return Container(
      alignment: Alignment.center,
      height: size.height,
      width: double.infinity,
      child: Stack(
        children: [
          Positioned(
            top: 45,
            left: 80,
            child: Image.asset(
              "assets/images/Logo_Đại_học_FPT.png",
              width: size.width * 0.6,
            ),
          ),
          Positioned(
            top: 180,
            left: 40,
            child: Image.asset(
              "assets/images/scheduler.png",
              width: size.width * 0.65,
            ),
          ),
          Positioned(
            bottom: 240,
            left: 30,
            child: Column(
              children: [
                Text(
                  "Lecturer Scheduler App",
                  style: TextStyle(
                    fontSize: 28,
                    color: Color.fromARGB(255, 5, 105, 122),
                    fontWeight: FontWeight.bold,
                  ),
                ),
                SizedBox(
                  height: 30,
                ),
                Container(
                  width: size.width * 0.8,
                  child: ClipRRect(
                    borderRadius: BorderRadius.circular(29),
                    // ignore: deprecated_member_use
                    child: FlatButton(
                      color: Colors.green,
                      padding:
                          EdgeInsets.symmetric(vertical: 0, horizontal: 30),
                      onPressed: signIn,
                      child: Row(
                        children: [
                          Container(
                            width: size.width * 0.1,
                            height: size.height * 0.08,
                            child: CircleAvatar(
                              radius: 56,
                              backgroundColor: Colors.white,
                              child: Padding(
                                padding:
                                    const EdgeInsets.all(2), // Border radius
                                child: ClipOval(
                                    child: Image.asset(
                                  'assets/images/Google__G__Logo.svg.png',
                                )),
                              ),
                            ),
                          ),
                          SizedBox(
                            width: 10,
                          ),
                          Text(
                            'Login with gmail',
                            style: TextStyle(
                              color: Colors.white,
                              fontSize: 23,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
