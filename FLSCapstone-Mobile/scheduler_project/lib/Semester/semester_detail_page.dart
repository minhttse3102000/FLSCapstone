// ignore_for_file: prefer_const_literals_to_create_immutables, sized_box_for_whitespace, no_leading_underscores_for_local_identifiers, must_be_immutable, avoid_unnecessary_containers

import 'package:flutter/material.dart';

import '../model/request.dart';
import '../model/semester.dart';
import '../model/user.dart';
import '../request/list_request.dart';
import '../services/api_handler.dart';
import 'components/semester_detail_card.dart';
import 'my_subject_page.dart';

class SemesterDetail extends StatefulWidget {
  final SemesterModel semesterModel;
  final UserModel userModel;
  const SemesterDetail({
    Key? key,
    required this.semesterModel,
    required this.userModel,
  }) : super(key: key);

  @override
  State<SemesterDetail> createState() => _SemesterDetailState();
}

class _SemesterDetailState extends State<SemesterDetail>
    with TickerProviderStateMixin {
  List<RequestModel> listRequest = [];
  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    getListRequest();
  }

  Future<void> getListRequest() async {
    listRequest = await APIHandler.getAllRequestByLecturer(
        lecturerId: widget.userModel.id.toString(),
        semesterId: widget.semesterModel.id.toString());
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    TabController _tabController = TabController(length: 2, vsync: this);

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          onPressed: () {
            Navigator.pop(context);
          },
          icon: Icon(
            Icons.arrow_back_ios,
            color: Colors.green[700],
          ),
        ),
        title: Text(
          'Semester Detail',
          style: TextStyle(
            color: Colors.green[700],
            fontWeight: FontWeight.bold,
          ),
        ),
        centerTitle: true,
        backgroundColor: Colors.white,
        elevation: 0,
      ),
      body: ListView(
        children: [
          SemesterDetailCard(
            semesterModel: widget.semesterModel,
          ),
          Container(
            child: Align(
              alignment: Alignment.center,
              child: TabBar(
                labelStyle: TextStyle(fontSize: 20),
                controller: _tabController,
                labelColor: Colors.black,
                isScrollable: true,
                indicator: CircleTabIndicator(color: Colors.black, radius: 4),
                //labelPadding: EdgeInsets.only(right: 20, left: 18),
                unselectedLabelColor: Colors.grey,
                tabs: [
                  Tab(
                    text: 'My Subject',
                  ),
                  Tab(text: 'View Request'),
                ],
              ),
            ),
          ),
          Container(
            width: double.maxFinite,
            height: MediaQuery.of(context).size.height * 0.667,
            child: TabBarView(
              controller: _tabController,
              children: [
                MySubjectScreen(userModel: widget.userModel),
                ListRequest(listRequest: listRequest),
              ],
            ),
          )
        ],
      ),
    );
  }
}

class CircleTabIndicator extends Decoration {
  final Color color;
  double radius;

  CircleTabIndicator({required this.color, required this.radius});

  @override
  BoxPainter createBoxPainter([VoidCallback? onChanged]) {
    return _CirclePainter(color: color, radius: radius);
  }
}

class _CirclePainter extends BoxPainter {
  final double radius;
  late Color color;
  _CirclePainter({required this.color, required this.radius});

  @override
  void paint(Canvas canvas, Offset offset, ImageConfiguration cfg) {
    late Paint _paint;
    _paint = Paint()..color = color;
    _paint = _paint..isAntiAlias = true;
    final Offset circleOffset =
        offset + Offset(cfg.size!.width / 2, cfg.size!.height - radius);
    canvas.drawCircle(circleOffset, radius, _paint);
  }
}
