using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    class EmployeeDtr
    {
        private List <String> record_id;
        private List <String> date;
        private List <String> employeeID;
        private List <String> am_in;
        private List <String> am_out;
        private List <String> pm_in;
        private List <String> pm_out;
        private List <String> ot_in;
        private List <String> ot_out;
        private List <String> duration;

        public EmployeeDtr ()
        {
            record_id = new List<String>();
            date = new List<String>();
            employeeID = new List<String>();
            am_in = new List<String>();
            am_out = new List<String>();
            pm_in = new List<String>();
            pm_out = new List<String>();
            ot_in = new List<String>();
            ot_out = new List<String>();
            duration = new List<String>();
        }

        public void setID (String id) {
            record_id.Add(id);
        }
        public void setDate (String key) {
            date.Add(key);
        }
        public void setEmployeeID (String id) {
            employeeID.Add(id);
        }
        public void parseKey(String key) {
            String [] key_component = key.Split(':');
            setEmployeeID(key_component[0]);
            setDate(key_component[1]);
        }
        public void setAMin (String s) {
            am_in.Add(s);
        }
        public void setAMout (String s) {
            am_out.Add(s);
        }
        public void setPMin(String s) {
            pm_in.Add(s);
        }
        public void setPMout(String s) {
            pm_out.Add(s);
        }
        public void setOTin(String s) {
            ot_in.Add(s);
        }
        public void setOTout(String s) {
            ot_out.Add(s);
        }
        public void setDuration(String s) {
            duration.Add(s);
        }
        public void addRecord (String key) {
            setID(key); //record_id
            parseKey(key); //date and employeeID

            //set to "null" and not null for testing purposes
            setAMin("null");
            setAMout("null");
            setPMin("null");
            setPMout("null");
            setOTin("null");
            setOTout("null");
            setDuration("null");
        }
        public int getRecordIDCount () {
            return record_id.Count;
        }
        public int getRecordIndex (String s) {
            int index = -1;
            for (int i = 0; i < getRecordIDCount(); i++) {
                if (record_id[i] == s) {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public bool hasRecordID(String s) {
            bool ans = false;
            int index = getRecordIndex(s);
            ans = (getRecordIndex(s) == -1 ? false : true);
            return ans;
        }
        public void modifyAMin(String key, String time) {
            int record_index = getRecordIndex(key);
            am_in[record_index] = time;
        }
        public void modifyAMout(String key, String time) {
            int record_index = getRecordIndex(key);
            am_out[record_index] = time;
        }
        public bool issetAMout (String key) {
            bool isset = false;
            int record_index = getRecordIndex(key);
            if (am_out[record_index] != "null") {
                isset = true;
            }
            return isset;
        }
        public void modifyPMin(String key, String time) {
            int record_index = getRecordIndex(key);
            pm_in[record_index] = time;
        }
        public void modifyPMout(String key, String time) {
            int record_index = getRecordIndex(key);
            pm_out[record_index] = time;
        }
        public void modifyOTin(String key, String time)
        {
            int record_index = getRecordIndex(key);
            ot_in[record_index] = time;
        }
        public void modifyOTout(String key, String time)
        {
            int record_index = getRecordIndex(key);
            ot_out[record_index] = time;
        }
        public void modifyDuration(int i, String d)
        {
            duration [i] = d;
        }
        public String computeDuration (int i, int sec_duration, int min_duration, int hour_duration, String timecode)
        {
            String[] time_in = am_in[i].Split(':');
            String[] time_out = am_out[i].Split(':');

            int hour_in = 0;
            int hour_out = 0;
            int min_in = 0;
            int min_out = 0;
            int sec_in = 0;
            int sec_out = 0;

            if (timecode == "AM") {
                time_in = am_in[i].Split(':');
                time_out = am_out[i].Split(':');
            }
            else if (timecode == "PM") {
                time_in = pm_in[i].Split(':');
                time_out = pm_out[i].Split(':');
            }
            else if (timecode == "OT") {
                time_in = ot_in[i].Split(':');
                time_out = ot_out[i].Split(':');
            }
            hour_in = Convert.ToInt32(time_in[0]);
            min_in = Convert.ToInt32(time_in[1]);
            sec_in = Convert.ToInt32(time_in[2]);

            hour_out = Convert.ToInt32(time_out[0]);
            min_out = Convert.ToInt32(time_out[1]);
            sec_out = Convert.ToInt32(time_out[2]);

            //compute duration [seconds]
            if (sec_out < sec_in) {
                if (min_out == 0) {
                    hour_out = hour_out - 1;
                    min_out = min_out + 60;
                }
                min_out = min_out - 1;
                sec_out = sec_out + 60;
            }
            sec_duration = sec_duration + (sec_out - sec_in);

            //compute duration [minutes]
            if (min_out < min_in) {
                hour_out = hour_out - 1;
                min_out = min_out + 60;
            }
            min_duration = min_duration + (min_out - min_in);

            //compute duration [hours]
            hour_duration = hour_duration + (hour_out - hour_in);
            return (hour_duration + ":" + min_duration + ":" + sec_duration);
        }
        public void computeTotalDuration () {
            for (int i = 0; i < getRecordIDCount(); i++) {
                int sec_duration = 0;
                int min_duration = 0;
                int hour_duration = 0;
                String ans = "";
                String[] duration_component;

                //compute AM duration
                if (am_in[i] != "null" && am_out[i] != "null") {
                    ans = computeDuration (i, sec_duration, min_duration, hour_duration, "AM");
                    duration_component = ans.Split(':');
                    hour_duration = Convert.ToInt32(duration_component[0]);
                    min_duration = Convert.ToInt32(duration_component[1]);
                    sec_duration = Convert.ToInt32(duration_component[2]);
                }

                //compute PM duration
                if (pm_in[i] != "null" && pm_out[i] != "null") {
                    ans = computeDuration(i, sec_duration, min_duration, hour_duration, "PM");
                    duration_component = ans.Split(':');
                    hour_duration = Convert.ToInt32(duration_component[0]);
                    min_duration = Convert.ToInt32(duration_component[1]);
                    sec_duration = Convert.ToInt32(duration_component[2]);
                }

                //compute OT duration
                if (ot_in[i] != "null" && ot_out[i] != "null") {
                    ans = computeDuration(i, sec_duration, min_duration, hour_duration, "OT");
                    duration_component = ans.Split(':');
                    hour_duration = Convert.ToInt32(duration_component[0]);
                    min_duration = Convert.ToInt32(duration_component[1]);
                    sec_duration = Convert.ToInt32(duration_component[2]);
                }
                int addend = 0;
                if (sec_duration >= 60) {
                    addend = sec_duration / 60;
                    min_duration = min_duration + addend;
                    sec_duration = sec_duration % 60;
                }
                if (min_duration >= 60) {
                    addend = min_duration / 60;
                    hour_duration = hour_duration + addend;
                    min_duration = min_duration % 60;
                }

                String duration = Convert.ToString(hour_duration) + ":" + Convert.ToString(min_duration) + ":" + Convert.ToString(sec_duration);
                modifyDuration(i, duration);
            }
        }
        public void writeToCSV(String output_repository) {
            computeTotalDuration();
            for (int i = 0; i < getRecordIDCount (); i++) {
                using (StreamWriter outputFile = new StreamWriter(output_repository + @"\DTR.csv", true)) {
                    outputFile.WriteLine(date[i] + "," + employeeID[i] +  "," + am_in[i] + "," + am_out[i] + "," + pm_in[i] + "," + pm_out[i] + "," + ot_in[i] + "," + ot_out[i] + "," + duration[i]);
                }                
            }
        }
    }
}
