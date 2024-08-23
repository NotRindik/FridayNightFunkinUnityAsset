//using system.collections;
//using system.collections.generic;
//using unityengine;
//using dialogue;

//namespace testing
//{
//    public class testing_aaa : monobehaviour
//    {
//        dialoguesystem ds;
//        textarchitect architect;

//        public textarchitect.buildmethod bm = textarchitect.buildmethod.instant;

//        string[] lines = new string[5]
//        {
//            "saassssssssssssd",
//            "sdadaaaaaaaaaaaa",
//            "sddddddddddddfdsfsdf",
//            "dffdfgsdfgdrthdfthjhjdfg",
//            "dsssssssssssssssssffffff"
//        };

//        private void start()
//        {
//            ds = dialoguesystem.instance;
//            architect = new textarchitect(ds.dialoguecontainer.dialoguetext);
//            architect.buildmethod = textarchitect.buildmethod.fade;
//            architect.speed = 0.5f;
//        }

//        private void update()
//        {
//            if (bm != architect.buildmethod)
//            {
//                architect.buildmethod = bm;
//                architect.stop();
//            }

//            if (input.getkeydown(keycode.s))
//                architect.stop();
//            string longline = "it's really long line that the make's no sense and it's for really long test mother fuker, we all like stuf, but i dont know how to continue this string, i cant imagine something, and so i think thats all";
//            if (input.getkeydown(keycode.space))
//            {
//                if (architect.isbuilding)
//                {
//                    if (!architect.hurryup)
//                    {
//                        architect.hurryup = true;
//                    }
//                    else
//                    {
//                        architect.forcecomplete();
//                    }
//                }
//                else
//                {
//                    architect.build(longline);
//                    //architect.build(lines[random.range(0, 5)]);
//                }
//            }
//            else if (input.getkeydown(keycode.a))
//            {
//                architect.append(longline);
//                //architect.append(lines[random.range(0, 5)]);
//            }
//        }
//    }
//}