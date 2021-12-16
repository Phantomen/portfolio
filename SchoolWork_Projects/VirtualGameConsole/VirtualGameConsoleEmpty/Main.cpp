#include <string>
#include "VGCVirtualGameConsole.h"
#include "Game.h"

using namespace std;

int VGCMain(const VGCStringVector &arguments) {
	Game game;
	game.run();
	return 0;
}



/*
//#include "VGCVirtualGameConsole.h"
//#include <string>
////#include <vector>
//
////Flytta över allt till Game.ccp
//
//using namespace std;

//
//struct Position {
//	
//	double xPosition;
//	double yPosition;
//};
//
//typedef std::vector<Position> PositionVector;
//
//
//struct Velocity : Position {
//
//	double xVelocity;
//	double yVelocity;
//};
//
//typedef std::vector<Velocity> VelocityVector;
//
//
////Lägg till pos
//struct Player {
//	
//	double xPosition;
//	double yPosition;
//	double Velocity;
//
//	double firePosition;
//};
//
//typedef std::vector<Player> PlayerVector;
//
////Lägg till vel
//struct Invader {
//	
//	double xPosition;
//	double yPosition;
//	double xVelocity;
//	double yVelocity = 10;
//
//	double firePosition;
//};
//
//typedef std::vector<Invader> InvaderVector;
//
////lägg till vel
//struct Bullet {
//	
//	double xPosition;
//	double yPosition;
//	double xVelocity;
//	double yVelocity = 10;
//};
//
//typedef std::vector<Bullet> BulletVector;
//
////lägg till pos
//struct Explosion {
//	
//	double xPosition;
//	double yPosition;
//	double lifetime = 1;
//};
//
//typedef std::vector<Explosion> ExplosionVector;


//int Health(int health, VGCFont font) {	//While health > 0:: beginLoop
//
//	while (VGCVirtualGameConsole::beginLoop()) {
//
//		if (VGCDisplay::beginFrame) {
//
//			const string        text = "Health: " + std::to_string(health);
//			const VGCColor      textColor = VGCColor(255, 255, 0, 0);
//			const VGCVector     textPosition = VGCVector(0, 0);
//			const VGCAdjustment textAdjustment = VGCAdjustment(0.0, 0.0);
//			VGCDisplay::renderString(
//				font,
//				text,
//				textColor,
//				textPosition,
//				textAdjustment);
//
//			VGCDisplay::endFrame();
//		}
//
//		VGCVirtualGameConsole::endLoop();
//	}
//
//	VGCDisplay::closeFont(font);
//	VGCVirtualGameConsole::finalize();
//
//	return 0;
//}
//
//int Score() {
//
//
//
//	return 0;
//}


//Skapa en klass som har position, "fart" och storlek som de andra objekten ärver

//Lägg till stjärnor som åker förbi i bakgrunden
//lägg till eld som åker ut bakom ditt rymdskepp som minskar i storlek
//Lägg till explosionen som minskar i storlek


int VGCMain(const VGCStringVector &arguments){
	
	//const string playerApplicationName = "Player";
	//const int    playerDISPLAY_WIDTH = 32;
	//const int    playerDISPLAY_HEIGHT = 32;
	//VGCVirtualGameConsole::initialize(playerApplicationName, playerDISPLAY_WIDTH, playerDISPLAY_HEIGHT);

	//const string invadersApplicationName = "Invaders";
	//const int    invadersDISPLAY_WIDTH = 32;
	//const int    invadersDISPLAY_HEIGHT = 32;
	//VGCVirtualGameConsole::initialize(invadersApplicationName, invadersDISPLAY_WIDTH, invadersDISPLAY_HEIGHT);

	//const string bulletsApplicationName = "Bullets";
	//const int    bulletsDISPLAY_WIDTH = 32;
	//const int    bulletsDISPLAY_HEIGHT = 32;
	//VGCVirtualGameConsole::initialize(bulletsApplicationName, bulletsDISPLAY_WIDTH, bulletsDISPLAY_HEIGHT);

	//const string explosionsApplicationName = "Explosions";
	//const int    explosionsDISPLAY_WIDTH = 32;
	//const int    explosionsDISPLAY_HEIGHT = 32;
	//VGCVirtualGameConsole::initialize(explosionsApplicationName, explosionsDISPLAY_WIDTH, explosionsDISPLAY_HEIGHT);


	int health = 100;
	//int score  = 0;
	//int highscore;
	//Läs in highscoren;
	//highscore = highscoren i texten

	const string applicationName = "Invader";
	const int    DISPLAY_WIDTH   = 500;
	const int    DISPLAY_HEIGHT  = 800;
	VGCVirtualGameConsole::initialize(applicationName, DISPLAY_WIDTH, DISPLAY_HEIGHT);

	const int FONT_SIZE = 24;
	VGCFont   font = VGCDisplay::openFont("Times New Roman", FONT_SIZE);

	while (VGCVirtualGameConsole::beginLoop()) {

		if (VGCDisplay::beginFrame()) {

			const VGCColor backgroundColor = VGCColor(255, 0, 0, 0);
			VGCDisplay::clear(backgroundColor);

			const VGCColor      textColor = VGCColor(255, 255, 0, 0);

			const string        text = "Health: " + std::to_string(health);
			const VGCVector     textPosition = VGCVector(0, 0);
			const VGCAdjustment textAdjustment = VGCAdjustment(0.0, 0.0);
			VGCDisplay::renderString(
				font,
				text,
				textColor,
				textPosition,
				textAdjustment);

			//Lägg till en string som skriver scoren och highscoren "backlänges" (ökar i längd åt höger istället för vänster)

			VGCDisplay::endFrame();
		}

		VGCVirtualGameConsole::endLoop();
	}

	VGCDisplay::closeFont(font);

	VGCVirtualGameConsole::finalize();

	return 0;
}

//int Health(VGCFont font) {	//While health > 0:: beginLoop
//
//	while (VGCVirtualGameConsole::beginLoop()) {
//
//		if (VGCDisplay::beginFrame) {
//
//			const string        text = "Health: " + std::to_string(health);
//			const VGCColor      textColor = VGCColor(255, 255, 0, 0);
//			const VGCVector     textPosition = VGCVector(0, 0);
//			const VGCAdjustment textAdjustment = VGCAdjustment(0.0, 0.0);
//			VGCDisplay::renderString(
//				font,
//				text,
//				textColor,
//				textPosition,
//				textAdjustment);
//
//			VGCDisplay::endFrame();
//		}
//
//		VGCVirtualGameConsole::endLoop();
//	}
//
//	VGCDisplay::closeFont(font);
//	VGCVirtualGameConsole::finalize();
//
//	return 0;
//}
////
////int Score() {
////
////
////
////	return 0;
////}
*/