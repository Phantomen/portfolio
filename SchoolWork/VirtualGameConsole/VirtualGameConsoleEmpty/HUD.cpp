#include "HUD.h"

using namespace std;


static const string filename = "HighScore.txt";
const int FONT_SIZE = 24;

static const VGCColor textColor = VGCColor(255, 255, 0, 0);

//Health string
string health = "Health: 100";
//const VGCVector     textPositionHealth = VGCVector(0, 0);
//const VGCAdjustment textAdjustmentHealth = VGCAdjustment(0.0, 0.0);

string score = "Score: 0";
//const VGCVector     textPositionScore = VGCVector(VGCDisplay::getWidth() - 1, 0);
//const VGCAdjustment textAdjustmentScore = VGCAdjustment(1.0, 0.0);

string highScore = "High score: 0";
//const VGCVector     textPositionHighScore = VGCVector(VGCDisplay::getWidth() - 1, 30);
//const VGCAdjustment textAdjustmentHighScore = VGCAdjustment(1.0, 0.0);



void HUD::updateStrings() {
	health = "Health: " + to_string(mHealth);
	score = "Score: " + to_string(mScore);
	highScore = "HighScore: " + to_string(mHighScore);
}


void HUD::printNewHighScore() {		//skriv ut när du stänger programmet?
	//mOutput.open(filename, ios::out | ios::trunc);
	//mOutput << mScore;
	//mOutput.close();

	mHighScore = mScore;
}

HUD::HUD() :
	mInput(filename.c_str()) {

	mHealth = 100;
	mScore = 0;

	mInput >> mHighScore;

	updateStrings();
}

HUD::~HUD() {

}

void HUD::update() {
	if (mScore > mHighScore) {
		printNewHighScore();
	}
	updateStrings();
}

void HUD::render() {
	const VGCVector     textPositionHealth = VGCVector(0, 0);
	const VGCAdjustment textAdjustmentHealth = VGCAdjustment(0.0, 0.0);

	const VGCVector     textPositionScore = VGCVector(VGCDisplay::getWidth() - 1, 0);
	const VGCAdjustment textAdjustmentScore = VGCAdjustment(1.0, 0.0);

	const VGCVector     textPositionHighScore = VGCVector(VGCDisplay::getWidth() - 1, 30);
	const VGCAdjustment textAdjustmentHighScore = VGCAdjustment(1.0, 0.0);

	VGCDisplay::renderString(
		mFont,
		health,
		textColor,
		textPositionHealth,
		textAdjustmentHealth);

	VGCDisplay::renderString(
		mFont,
		score,
		textColor,
		textPositionScore,
		textAdjustmentScore);

	VGCDisplay::renderString(
		mFont,
		highScore,
		textColor,
		textPositionHighScore,
		textAdjustmentHighScore);
}

void HUD::initialize() {
	mFont = VGCDisplay::openFont("Times New Roman", FONT_SIZE);
}

void HUD::finalize() {
	mOutput.open(filename, ios::out | ios::trunc);
	mOutput << mScore;
	mOutput.close();

	VGCDisplay::closeFont(mFont);
}

void HUD::giveDamage(int dmg) {
	mHealth -= dmg;
}

int HUD::getHealth() {
	return mHealth;
}

void HUD::giveScore(int scr) {
	mScore += scr;
}