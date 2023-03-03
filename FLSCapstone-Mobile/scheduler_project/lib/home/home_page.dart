// ignore_for_file: avoid_unnecessary_containers, prefer_const_literals_to_create_immutables, constant_identifier_names, non_constant_identifier_names, prefer_const_constructors_in_immutables, use_build_context_synchronously

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:scheduler_project/home/components/my_body.dart';
import 'package:scheduler_project/home/components/my_drawer_header.dart';
import 'package:scheduler_project/model/user.dart';

import '../google/google_sigin_api.dart';
import '../screen/login_screen.dart';
import '../semester/semester_page.dart';
import '../widgets/main_drawer.dart';

class HomePage extends StatefulWidget {
  final UserModel userModel;
  HomePage({
    required this.userModel,
  });
  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  var currentPage = DrawerSection.home;
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('FLS Scheduler'),
        centerTitle: true,
        backgroundColor: Colors.green[700],
        actions: [
          TextButton(
            onPressed: () async {
              await GoogleSignInApi.logout();
              Navigator.of(context).pushReplacement(MaterialPageRoute(
                builder: (context) => LoginScreen(),
              ));
            },
            child: Text(
              'Logout',
              style: TextStyle(color: Colors.white),
            ),
          )
        ],
      ),
      drawer: Drawer(
        child: SingleChildScrollView(
          child: Container(
            child: Column(
              children: [
                ChangeNotifierProvider.value(
                  value: widget.userModel,
                  child: MyHeaderDrawer(),
                ),
                //MyDrawerList(),
                MainDrawer(),
              ],
            ),
          ),
        ),
      ),
      body: MyBody(userModel: widget.userModel),
    );
  }

  DrawerList() {
    return Container(
      padding: EdgeInsets.only(top: 15),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        //show the list of menu drawer
        children: [
          menuItem(1, 'Home', Icons.dashboard_outlined,
              currentPage == DrawerSection.home ? true : false),
          menuItem(2, 'Scheduler', Icons.people_alt_outlined,
              currentPage == DrawerSection.scheduler ? true : false),
          menuItem(3, 'Semester', Icons.access_time,
              currentPage == DrawerSection.semester ? true : false),
          menuItem(4, 'My Department', Icons.business_outlined,
              currentPage == DrawerSection.department ? true : false),
          menuItem(5, 'Notification', Icons.notifications,
              currentPage == DrawerSection.notification ? true : false),
          menuItem(6, 'Profile', Icons.person,
              currentPage == DrawerSection.profile ? true : false),
          Padding(
            padding: const EdgeInsets.only(left: 20),
            child: TextButton.icon(
              onPressed: () async {
                await GoogleSignInApi.logout();
                Navigator.of(context).pushReplacement(MaterialPageRoute(
                  builder: (context) => LoginScreen(),
                ));
              },
              icon: Icon(
                Icons.logout_outlined,
                color: Colors.black,
                size: 20,
              ),
              label: Text(
                'Logout',
                style: TextStyle(
                  color: Colors.black,
                  fontSize: 16,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  logout() async {
    await GoogleSignInApi.logout();
    Navigator.of(context).pushReplacement(MaterialPageRoute(
      builder: (context) => LoginScreen(),
    ));
  }

  menuItem(int id, String title, IconData icon, bool selected) {
    return Material(
      color: selected ? Colors.grey[300] : Colors.transparent,
      child: InkWell(
        onTap: () {
          Navigator.pop(context);
          setState(() {
            //currentPage = DrawerSection.events;
            if (id == 1) {
              currentPage = DrawerSection.home;
            } else if (id == 2) {
              currentPage = DrawerSection.scheduler;
            } else if (id == 3) {
              currentPage = DrawerSection.semester;
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) =>
                      SemesterPage(userModel: widget.userModel),
                ),
              );
            } else if (id == 4) {
              currentPage = DrawerSection.department;
              print(currentPage);
            } else if (id == 5) {
              currentPage = DrawerSection.notification;
              print(currentPage);
            } else if (id == 6) {
              currentPage = DrawerSection.profile;
              print(currentPage);
            }
          });
        },
        child: Padding(
          padding: EdgeInsets.all(10),
          child: Row(
            children: [
              Expanded(
                child: Icon(
                  icon,
                  size: 20,
                  color: Colors.black,
                ),
              ),
              Expanded(
                flex: 5,
                child: Text(
                  title,
                  style: TextStyle(
                    color: Colors.black,
                    fontSize: 16,
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}

enum DrawerSection {
  home,
  scheduler,
  semester,
  department,
  notification,
  profile,
  logout,
}
