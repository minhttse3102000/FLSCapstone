import 'package:flutter/material.dart';
import 'package:flutter/src/foundation/key.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:scheduler_project/widgets/login.dart';

import '../home/home_page.dart';
import 'drawer.dart';

class MainDrawer extends StatefulWidget {
  const MainDrawer({Key? key}) : super(key: key);

  @override
  State<MainDrawer> createState() => _MainDrawerState();
}

class _MainDrawerState extends State<MainDrawer> {
  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          DrawerListTile(
            imgpath: "home.png",
            name: "Home",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
          DrawerListTile(
            imgpath: "calendar.png",
            name: "Timetable",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
          DrawerListTile(
            imgpath: "semester.png",
            name: "Semester",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
          DrawerListTile(
            imgpath: "department.png",
            name: "Department",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
          DrawerListTile(
            imgpath: "subject.png",
            name: "Subject",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
          DrawerListTile(
            imgpath: "profile-item.png",
            name: "Profile",
            ontap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (BuildContext context) => Login(),
                ),
              );
            },
          ),
        ],
      ),
    );
  }
}
