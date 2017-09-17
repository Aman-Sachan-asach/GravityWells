using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour 
{
	/*
	[Tooltip("filename for logging")]
	private string logfileName = "DataLog_Whack_A_Mole_Test";
	private string logSubject = "_Alpha_";
	private string logNumber = "0";
	private string filename;
	public string filepath;

	private StreamWriter file = null;

	private static int bufferSize = 50000; //number of elements
	private int bufferIndex = 0;

	private float[] Buffer_time = new float[bufferSize*2]; //spawnbeforeTime,
	//displaytime

	private float[] Buffer_angularPosition = new float[bufferSize*3];//angular_position_min, 
	//angular_position_max,
	//angular_position

	private float[] Buffer_degreesPerCue = new float[bufferSize];//degreesPerCue
	private float[] Buffer_frequency = new float[bufferSize]; //number of times the cue showed up before the mole did

	private float[] Buffer_angleSubtended = new float[bufferSize];//angleSubtended

	private float[] Buffer_colorRGBA = new float[bufferSize*12]; //r_min,g_min,b_min,a_min
	//r_max,g_max,b_max,a_max
	//r ,g ,b ,a 

	private bool[] Buffer_emissionOnOff = new bool[bufferSize];//emission_on/off, 
	private float[] Buffer_emission = new float[bufferSize*3];//emissiveIntensity,
	//eI_lowerbound, 
	//eI_upperbound

	private float[] Buffer_targetSeen = new float[bufferSize];//how many milliseconds the gaze rested upon the target visualCue
	private float[] Buffer_visualCueLife = new float[bufferSize];//how many milliseconds the visualCue was displayed onscreen
	private float[] Buffer_responseTime = new float[bufferSize];//time from when the visualCue Shows up to when the user rests their gaze upon it

	private bool[] Buffer_userResponded = new bool[bufferSize];//gaze held at visual Cue for a time period above some threshold?
	private bool[] Buffer_userCorrect = new bool[bufferSize];//camera direction was within some proximity threshold to where a mole would pop up

	private int[] Buffer_moleHit = new int[bufferSize];//0 if miss, 1 if hit, 2 if a mole didnt exist to hit

	public static DataLogger dataLogger = null;
	// Use this for initialization
	void Awake ()
	{
		if (dataLogger == null)
		{
			dataLogger = this;
		}            
		else if(dataLogger !=this)
		{
			Destroy(gameObject); //another dataLogger already exists
		}

		//dataLogger isn't destroyed while moving through scenes
		DontDestroyOnLoad(gameObject);
	}

	public void WriteEventsToBuffers(float time_spawnbefore, float time_displaytime, float ang, float ang_min,
		float ang_max, float degsPerCue, float frequency, float ang_subtended,
		Color color_curr, Color color_min, Color color_max, bool emission_OnOff,
		float emission_intensity, float emission_lowerbound, float emission_upperbound,
		float t_seen, float cueLife, float time_response, bool user_responded, bool user_correct, 
		int mole_hit)
	{
		Buffer_time[bufferIndex*2] = time_spawnbefore;
		Buffer_time[bufferIndex*2 + 1] = time_displaytime;

		Buffer_angularPosition[bufferIndex*3] = ang;
		Buffer_angularPosition[bufferIndex*3 + 1] = ang_min;
		Buffer_angularPosition[bufferIndex*3 + 2] = ang_max;

		Buffer_degreesPerCue[bufferIndex] = degsPerCue;
		Buffer_frequency[bufferIndex] = frequency;

		Buffer_angleSubtended[bufferIndex] = ang_subtended;

		Buffer_colorRGBA[bufferIndex*12] = color_curr.r;
		Buffer_colorRGBA[bufferIndex*12+1] = color_curr.g;
		Buffer_colorRGBA[bufferIndex*12+2] = color_curr.b;
		Buffer_colorRGBA[bufferIndex*12+3] = color_curr.a;
		Buffer_colorRGBA[bufferIndex*12+4] = color_min.r;
		Buffer_colorRGBA[bufferIndex*12+5] = color_min.g;
		Buffer_colorRGBA[bufferIndex*12+6] = color_min.b;
		Buffer_colorRGBA[bufferIndex*12+7] = color_min.a;
		Buffer_colorRGBA[bufferIndex*12+8] = color_max.r;
		Buffer_colorRGBA[bufferIndex*12+9] = color_max.g;
		Buffer_colorRGBA[bufferIndex*12+10] = color_max.b;
		Buffer_colorRGBA[bufferIndex*12+11] = color_max.a;

		Buffer_emissionOnOff[bufferIndex] = emission_OnOff;
		Buffer_emission[bufferIndex*3] = emission_intensity;
		Buffer_emission[bufferIndex*3+1] = emission_lowerbound;
		Buffer_emission[bufferIndex*3+2] = emission_upperbound;

		Buffer_targetSeen[bufferIndex] = t_seen;
		Buffer_visualCueLife[bufferIndex] = cueLife;
		Buffer_responseTime[bufferIndex] = time_response;

		Buffer_userResponded[bufferIndex] = user_responded;
		Buffer_userCorrect[bufferIndex] = user_correct;

		Buffer_moleHit[bufferIndex] = mole_hit;
		bufferIndex++;
	}

	public void WriteBufferstoFile()
	{
		string currentDirectoryPath = Path.GetDirectoryName(Application.dataPath);
		string lognumberfile = currentDirectoryPath + "/DataLogs/" + "_logNumber_.txt";

		if (!File.Exists(lognumberfile))
		{
			File.Create(lognumberfile).Dispose();

			using (FileStream f = new FileStream(lognumberfile, FileMode.Append, FileAccess.Write))
			{
				using (StreamWriter s = new StreamWriter(f))
				{
					s.WriteLine(logNumber);
				}
			}
		}
		else
		{
			logNumber = File.ReadAllText(lognumberfile);
			// do error handling here to make sure logNumber is a number in string format
			int lognum = Convert.ToInt32(logNumber);
			lognum++;

			logNumber = lognum.ToString();
			File.WriteAllText(lognumberfile, logNumber);
		}

		//csv files are easy to read as spreadsheets and can be opened by most spreadsheet programs like excel
		//every row is separated by a newline
		//every column is separated by a comma
		filename = logfileName + "_" + logSubject + "_" + logNumber +
			"_Date_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_Time_" +
			DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".csv";
		filepath = currentDirectoryPath + "/DataLogs/" + filename;

		using (file = new StreamWriter(filepath, true))
		{
			//write everything to the file
			file.WriteLine("Test Data from the Subliminal Eye Direction WhackAMole Game\n");
			file.WriteLine("Spawn Before Time, Display Time, " + 
				"Angular Position, Angular Position Min, Angular Position Max, " + 
				"Angle Subtended, " +
				"Color rgba, Color min rgba, Color max rgba, " +
				"Emissive On/Off, Emissive Intensity, Emissive Intensity min, Emissive Intensity max, " + 
				"user response Time, target Seen Time, Life of Cue on Screen, gazeLength/cueLife," + 
				"user Responded, " + 
				"user Correct, mole Hit," +
				"degrees per visual Cue, Cue frequency" + "\n");
			for (int i = 0; i <= bufferIndex; i++) // write specified lines of buffer to file
			{
				file.Write(Buffer_time[i*2] + "," + 
					Buffer_time[i*2 + 1] + ",");                           

				file.Write(Buffer_angularPosition[i*3] + "," + 
					Buffer_angularPosition[i*3 + 1] + "," + 
					Buffer_angularPosition[i*3 + 2] + ",");

				file.Write(Buffer_angleSubtended[i] + ",");

				file.Write(Buffer_colorRGBA[i*12] + "|" + Buffer_colorRGBA[i*12 + 1] + "|" + 
					Buffer_colorRGBA[i*12 + 2] + "|" + Buffer_colorRGBA[i*12 + 3] + "," +
					Buffer_colorRGBA[i*12 + 4] + "|" + Buffer_colorRGBA[i*12 + 5] + "|" +
					Buffer_colorRGBA[i*12 + 6] + "|" + Buffer_colorRGBA[i*12 + 7] + "," +
					Buffer_colorRGBA[i*12 + 8] + "|" + Buffer_colorRGBA[i*12 + 9] + "|" +
					Buffer_colorRGBA[i*12 + 10] + "|" + Buffer_colorRGBA[i*12 + 11] + ",");

				file.Write(Buffer_emissionOnOff[i] + ",");

				file.Write(Buffer_emission[i*3] + "," +
					Buffer_emission[i*3 + 1] + "," +
					Buffer_emission[i*3 + 2] + ",");

				file.Write(Buffer_targetSeen[i] + ",");
				file.Write(Buffer_visualCueLife[i] + ",");
				file.Write(Buffer_targetSeen[i]*100/Buffer_visualCueLife[i] + ",");
				file.Write(Buffer_responseTime[i] + ","); 
				file.Write(Buffer_userResponded[i] + ",");
				file.Write(Buffer_userCorrect[i] + ",");
				file.Write(Buffer_moleHit[i] + ",");
				file.Write(Buffer_degreesPerCue[i] + ",");
				file.WriteLine(Buffer_frequency[i] + ",");
			}
		}

		// flush buffers and reset index
		Array.Clear(Buffer_time, 0, bufferIndex*2);
		Array.Clear(Buffer_angularPosition, 0, bufferIndex*3);
		Array.Clear(Buffer_degreesPerCue, 0, bufferIndex);
		Array.Clear(Buffer_frequency, 0, bufferIndex);
		Array.Clear(Buffer_angleSubtended, 0, bufferIndex);
		Array.Clear(Buffer_colorRGBA, 0, bufferIndex*12);
		Array.Clear(Buffer_emissionOnOff, 0, bufferIndex);
		Array.Clear(Buffer_emission, 0, bufferIndex*3);
		Array.Clear(Buffer_targetSeen, 0, bufferIndex);
		Array.Clear(Buffer_visualCueLife, 0, bufferIndex);
		Array.Clear(Buffer_responseTime, 0, bufferIndex);
		Array.Clear(Buffer_userResponded, 0, bufferIndex);
		Array.Clear(Buffer_userCorrect, 0, bufferIndex);
		Array.Clear(Buffer_moleHit, 0, bufferIndex);

		bufferIndex = 0;
	}

	// Update is called once per frame
	void Update()
	{}

	void OnApplicationQuit()
	{
		print("Application ending after " + Time.time + " seconds");
		WriteBufferstoFile();
		//GetComponent<DataGrapher>().graphFromData(filepath);
		print("DataLog created");
	}
	*/
}
