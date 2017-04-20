using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public abstract class Highscores : MonoBehaviour {
	
	public static  int highscoreLength = 10;
	public static  int amountOfLevels = 5;
	private static int defaultDummyScore = 0;
	private static string highscorePath = @"/Highscores.xml";
	private static string tempHighscorePath = @"/Highscores1.xml";
	private static FileStream reader;
	private static XmlDocument xmlDocument;
	private static bool isPrepared = false;
	
	
	//The method assures that a xml file is either loaded, or it is created and formatted correctly 
	//and then loaded.
	private static void Prepare(){
		//Highscore entries from previous game already existing?
		if(!(File.Exists(highscorePath))){
			//Create xml file, since one did not exist.
			XmlTextWriter tw = new XmlTextWriter(highscorePath, System.Text.Encoding.UTF8);
			tw.Flush();
			tw.Formatting = Formatting.Indented;
			tw.WriteStartDocument();
			tw.WriteStartElement("Highscores");
			//Make highscoreentries for each level.
			tw.WriteStartElement("Level");
			for(int i = 1; i <= amountOfLevels; i++){
				tw.WriteStartElement("Level" + i);
				for(int j = 1; j <= highscoreLength; j++){
					tw.WriteStartElement("Highscore" + j);
					tw.WriteElementString("Scoreholder", "Dummy" + j.ToString());
					tw.WriteElementString("Score", defaultDummyScore.ToString());
					tw.WriteEndElement();
				}
				tw.WriteEndElement();
			}
			tw.WriteEndElement();
			//Make global highscoreentries
			tw.WriteStartElement("Global");
			for(int i = 1; i <= highscoreLength; i++){
				tw.WriteStartElement("Highscore" + i);
				tw.WriteElementString("Scoreholder", "Dummy" + i.ToString());
				tw.WriteElementString("Score", defaultDummyScore.ToString());
				tw.WriteEndElement();
			}
			tw.WriteEndElement();	
			tw.WriteEndElement();
			tw.Flush();
			tw.Close();
		}
		reader = new FileStream(highscorePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		xmlDocument = new XmlDocument();
		xmlDocument.Load(reader);
		
		isPrepared = true;
	}
	
	//This method gets the highscores(name, score) for a parametergiven level.
	//The levels start at level 1.
	public static string[,,] GetLevelHighscores(){
		if(isPrepared == false){
			Prepare();
		}
		
		XmlNodeList lvlNodes = xmlDocument.GetElementsByTagName("Level");
		
		string[,,] highscoreEntries = new string[amountOfLevels, highscoreLength, 2];
		for(int i = 0; i < amountOfLevels; i++){ //loop through levels
			for(int j = 0; j < highscoreLength; j++){ //loop through all highscores for each level
				highscoreEntries[i, j, 0] = lvlNodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[0].InnerText; //Scoreholder name
				highscoreEntries[i, j, 1] = lvlNodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[1].InnerText; //Scoreholder score	
			}
		}
		return highscoreEntries;
	}
	
	public static string[,,] GetTotalHighscore(){
		if(isPrepared == false){
			Prepare();
		}
		XmlNodeList globalNodes = xmlDocument.GetElementsByTagName("Global");
		
		string[,,] highscoreEntries = new string[1, highscoreLength, 2];
		for(int i = 0; i < highscoreLength; i++){ //loop through all global highscores
				highscoreEntries[0, i, 0] = globalNodes[0].ChildNodes[i].ChildNodes[0].InnerText; //Scoreholder name
				highscoreEntries[0, i, 1] = globalNodes[0].ChildNodes[i].ChildNodes[1].InnerText; //Scoreholder score	
		}
		return highscoreEntries;
	}
	
	//Adds a highscore(name, score) to a level. The levels start at 1.
	//If inputting level 0, it adds a global score
	public static void AddScore(string playerName, int score, int level){
		if(isPrepared == false){
			Prepare();
		}
		
		XmlNodeList lvlNodes;
		if(level == 0)
			 lvlNodes = xmlDocument.GetElementsByTagName("Global");
		else
			 lvlNodes = xmlDocument.GetElementsByTagName("Level" + level);
		
		//This are the values to test whether they are bigger than the current selected highscore place
		string testHighScoreName = playerName;
		int testHighScorePoints = score;
		
		//temps needed for swapping scores
		string tempHighScoreName;
		string tempHighScorePoints;
		
		for(int i = 0; i < lvlNodes[0].ChildNodes.Count; i++){
			if(testHighScorePoints > int.Parse(lvlNodes[0].ChildNodes[i].ChildNodes[1].InnerText)){
				tempHighScoreName = lvlNodes[0].ChildNodes[i].ChildNodes[0].InnerText; //Old scoreholder name
				tempHighScorePoints = lvlNodes[0].ChildNodes[i].ChildNodes[1].InnerText; //Old scoreholder score
				
				lvlNodes[0].ChildNodes[i].ChildNodes[0].InnerText = testHighScoreName; //new scoreholder name
				lvlNodes[0].ChildNodes[i].ChildNodes[1].InnerText = testHighScorePoints.ToString(); //new scoreholder score
				
				testHighScoreName = tempHighScoreName; //old score, that is now tested in the subsequent highscoreplaces, to see if there is space for it there
				testHighScorePoints = int.Parse(tempHighScorePoints); //old score, that is now tested in the subsequent highscoreplaces, to see if there is space for it there
			}
		} 
		
		//Since editing in a xml file(with a filestreamwriter atleast.. dont have time to research other options)
		//Just overwrites in the old xml file we will, if the new scorename/points was bigger than the old ones
		//that hold the scorespace, write outside the bounds of the score.. therefore we make a new xml
		//document instead to avoid this.. troublesome = yes. probably an easier way = yes.
		if (File.Exists((tempHighscorePath))) {
            File.Delete(tempHighscorePath);    
        }
		
		//recreate the xml file
		FileStream tempFileStream = new FileStream(tempHighscorePath, FileMode.Create);
        tempFileStream.Close();

		//open the new xml file
        FileStream writer = new FileStream(tempHighscorePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
		
		//replace the old xml file with the new
        xmlDocument.Save(writer);
        writer.Close();
        reader.Close();
        //File.Delete(highscorePath);
        //File.Move(tempHighscorePath, highscorePath);
		
		xmlDocument = new XmlDocument();
		reader = new FileStream(highscorePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		xmlDocument.Load(reader);
		
	}
	
	
	public static void ResetHighScores(){
		if(!isPrepared){
			Prepare();
		}
		reader.Close();
		File.Delete(highscorePath);
		Prepare();
	}
}
