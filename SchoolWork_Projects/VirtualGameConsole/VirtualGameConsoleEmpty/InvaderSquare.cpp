#include "InvaderSquare.h"

using namespace std;

static const int invSquSpeed = 1;
static const int invSquBulletSpeed = 2;


static const int image_size = 32;
static const int HEIGHT = 32;
static int WIDTH = 32;
static const int SCOREWORTH = 100;
static const string filename = "InvaderBox.png";
static VGCImage image;

int fpsSqu = 0;
int firerateSqu = 0;


static VGCVector getRandomPosition() {
	const int MIN_X = WIDTH / 2;
	const int MAX_X = VGCDisplay::getWidth() - WIDTH / 2;
	const int Y = -HEIGHT / 2;
	const int X = VGCRandomizer::getInt(MIN_X, MAX_X);
	const VGCVector position(X, Y);
	return position;
}

InvaderSquare::InvaderSquare(int fps, Vectors *vec, HUD *h) :
	vector(vec), hud(h),
	mPosition(getRandomPosition()) {
	fpsSqu = fps;
	firerateSqu = fpsSqu;

	mSpeedX = VGCRandomizer::getInt(0, 2 * invSquSpeed);
	mSpeedY = invSquSpeed;
	mDir = VGCRandomizer::getInt(0, 1);
	if (mDir == 1) {
		mSpeedX = -mSpeedX;
	}

	mTimeBetweenShots = 0;
}


InvaderSquare::~InvaderSquare() {
}

void InvaderSquare::update() {
	const int MIN_X = WIDTH / 2;
	const int MAX_X = VGCDisplay::getWidth() - WIDTH / 2;
	int x = mPosition.getX();

	if (x <= MIN_X) {
		if (mSpeedX < 0)
			mSpeedX = -mSpeedX;
	}

	else if (x >= MAX_X) {
		if (mSpeedX > 0)
			mSpeedX = -mSpeedX;
	}

	x += mSpeedX;

	mPosition.setX(x);



	const int MAX_Y = VGCDisplay::getHeight() + HEIGHT / 2;
	int y = mPosition.getY();
	y += mSpeedY;
	if (y > MAX_Y) {
		mPosition = getRandomPosition();
		hud->giveScore(-SCOREWORTH / 2);
		mSpeedX = VGCRandomizer::getInt(0, 2);
		mDir = VGCRandomizer::getInt(0, 1);
		if (mDir == 1)
			mSpeedX = -mSpeedX;
	}
	else {
		mPosition.setY(y);
	}

	mTimeBetweenShots++;
	if (mTimeBetweenShots >= firerateSqu) {
		vector->addBulletInvaders(new Bullet(mPosition, invSquBulletSpeed, 0));
		mTimeBetweenShots = 0;
	}
}

void InvaderSquare::render() {
	VGCVector index(0, 0);
	VGCAdjustment adjustment(0.5, 0.5);
	VGCDisplay::renderImage(image, index, mPosition, adjustment);
}

void InvaderSquare::initialize() {
	image = VGCDisplay::openImage(filename, 1, 1);
}

void InvaderSquare::finalize() {
	VGCDisplay::closeImage(image);
}

VGCVector InvaderSquare::getPosition() const {
	return mPosition;
}

int InvaderSquare::getSize() const {
	return image_size;
}

void InvaderSquare::destroy(Entity *ent) {
	hud->giveScore(SCOREWORTH);
	vector->addExplosion(new Explosion(mPosition));
	delete ent;
}

string InvaderSquare::getType() const {
	return mType;
}