using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    class Program
    {
        static void Main(string[] args)
        {
        	//necessary variables from text file
            String line_EnNo = "";
            String line_Mode = "";
            String line_DateTime = "";

            List<String> header = new List<String>();

            System.IO.StreamReader file = new System.IO.StreamReader(@"D:\00nikit\ACUMATICA\Exercise\4\GLG_001.TXT");
            string output_repository = (@"D:\00nikit\ACUMATICA\Exercise\4\");

            int counter = 0;
            String line = "";

            EmployeeDtr dtr = new EmployeeDtr();
            while ((line = file.ReadLine()) != null)
            {
                String[] str_arr = line.Split('\t');
                if (counter == 0) {
            		for (int i = 0; i < str_arr.Length; i++) {
                        header.Add(str_arr[i]);
                    }
                    //print new header in a csv file
                    using (StreamWriter outputFile = new StreamWriter(output_repository + @"\DTR.csv")) {
                        outputFile.WriteLine("Date, Employee ID, AM IN, AM OUT, PM IN, PM OUT, OT IN, OT OUT, Duration");
                    }
            	}
            	else {
                    //get values
                    for (int i = 0; i < str_arr.Length; i++) {
                        if (header[i] == "EnNo")
                            line_EnNo = str_arr[i];
                        if (header[i] == "Mode")
                            line_Mode = str_arr[i];
                        if (header[i] == "DateTime")
                            line_DateTime = str_arr[i];
                    }                    	
                }
                if (line_Mode == "5" || line_Mode == "6") {
                    String[] dateTime = line_DateTime.Split(' ');
                    String date = dateTime[0];
                    String time = dateTime[1];
                    String key = line_EnNo + ":" + date;
                    if (!dtr.hasRecordID(key)) {
                    	/* Records record_id, date, and employeeID
                    	Set all other attributes to null for further processing of next lines
                    	*/
                    	dtr.addRecord(key);
                    }
                    TimeSetter(line_Mode, key, date, time, dtr);
                }                
            	System.Console.WriteLine(line);
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);

            //Write to CSV File
            dtr.writeToCSV(output_repository);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
        private static void TimeSetter(String mode, String key, String date, String time, EmployeeDtr dtr) {
        	String [] time_component = time.Split(':');
        	int hh = Convert.ToInt32(time_component[0]);

        	if (mode == "5") { //IN
        		if (hh < 12) { //AM IN
        			dtr.modifyAMin(key, time);
        		}
        		else if (hh >= 12 && hh < 18) { //PM IN
        			dtr.modifyPMin(key, time);
        		}
        	}
        	else if (mode == "6") { //OUT
        		if (hh <= 13) { //AM OUT
                    if (hh <= 12) {
                        dtr.modifyAMout(key, time);
                    }
                    else if (hh > 12) {
                        //check first if AM OUT is null, if not, assign time to it
                        if(!dtr.issetAMout(key)) {
                            dtr.modifyAMout(key, time);
                        }
                    }
        		}
        		else if (hh >= 13 && hh <= 23) { //PM OUT, OT IN, OT OUT
                    if (hh >= 18) { //OVERTIME
                        dtr.modifyPMout(key, "18:00:00");
                        dtr.modifyOTin(key, "18:00:00");
                        dtr.modifyOTout(key, time);
                    }
                    else { //UNDERTIME
                        dtr.modifyPMout(key, time);
                    }
        		}
        	}
        }
    }
}