import 'package:flutter/material.dart';
import '../widgets/login.dart';

class LoginScreen extends StatelessWidget {
  static String routeName = 'LoginScreen';
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Login(),
    );
  }
}
