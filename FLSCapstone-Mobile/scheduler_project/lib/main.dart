import 'package:flutter/material.dart';
import 'package:scheduler_project/constants.dart';
import 'package:scheduler_project/test_drawer.dart';
import 'package:scheduler_project/widgets/login.dart';
import './screen/login_screen.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Lecturer Scheduler',
      theme: ThemeData(
        primaryColor: kPrimaryColor,
        scaffoldBackgroundColor: Colors.white,
        dialogTheme: DialogTheme(
          backgroundColor: Colors.white,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(24),
          ),
        ),
      ),
      home: LoginScreen(), //LoginScreen
      //initialRoute: LoginScreen.routeName,
      routes: {
        LoginScreen.routeName: (ctx) => LoginScreen(),
        Login.routeName: (ctx) => Login(),
      },
    );
  }
}
