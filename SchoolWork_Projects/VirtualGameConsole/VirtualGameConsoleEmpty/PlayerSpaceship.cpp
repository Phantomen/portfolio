#include "PlayerSpaceship.h"


using namespace std;
static const int speed = 2;
static const int bulletSpeed = 4;
static const int image_size = 32;
static const int height = 32;
static const int width = 32;
static const string filename = "PlayerSpaceship.png";
static VGCImage image;

int fpsPlayer = 0;
int fireratePlayer = 0;
int currentTimeBetweenShotsPlayer = 0;


PlayerSpaceship::PlayerSpaceship(int fps, Vectors *vec, HUD *h) :
	vector(vec), hud(h),
	mPosition(VGCDisplay::getWidth() / 2, 2 * VGCDisplay::getHeight() / 3) {
	fpsPlayer = fps;
	fireratePlayer = fpsPlayer / 2;
	currentTimeBetweenShotsPlayer = fireratePlayer;
}


PlayerSpaceship::~PlayerSpaceship() {
}

void PlayerSpaceship::update() {
	const int MIN_X = width / 2;
	const int MIN_Y = height / 2;
	const int MAX_X = VGCDisplay::getWidth() - width / 2;
	const int MAX_Y = VGCDisplay::getHeight() - height / 2;
	int x = mPosition.getX();
	int y = mPosition.getY();

	if (VGCKeyboard::isPressed(VGCKey::D_KEY) || VGCKeyboard::isPressed(VGCKey::ARROW_RIGHT_KEY)) {
		x += speed;
		if (MAX_X < x) {
			x = MAX_X;
		}
	}
	else if (VGCKeyboard::isPressed(VGCKey::A_KEY) || VGCKeyboard::isPressed(VGCKey::ARROW_LEFT_KEY)) {
		x -= speed;
		if (x < MIN_X) {
			x = MIN_X;
		}
	}
	if (VGCKeyboard::isPressed(VGCKey::W_KEY) || VGCKeyboard::isPressed(VGCKey::ARROW_UP_KEY)) {
		y -= speed;
		if (y < MIN_Y) {
			y = MIN_Y;
		}
	}
	else if (VGCKeyboard::isPressed(VGCKey::S_KEY) || VGCKeyboard::isPressed(VGCKey::ARROW_DOWN_KEY)) {
		y += speed;
		if (MAX_Y < y) {
			y = MAX_Y;
		}
	}
	mPosition.setX(x);
	mPosition.setY(y);

	currentTimeBetweenShotsPlayer++;
	//Shoots the bullets
	if(VGCKeyboard::isPressed(VGCKey::SPACE_KEY)) {
		if (currentTimeBetweenShotsPlayer >= fireratePlayer) {
			VGCVector shotOrgin(mPosition.getX(), mPosition.getY() - image_size / 2);

			vector->addBulletPlayer(new Bullet(shotOrgin, -bulletSpeed + (bulletSpeed / 4), -bulletSpeed / 4));
			vector->addBulletPlayer(new Bullet(shotOrgin, -bulletSpeed, 0));
			vector->addBulletPlayer(new Bullet(shotOrgin, -bulletSpeed + (bulletSpeed / 4), bulletSpeed / 4));
			currentTimeBetweenShotsPlayer = 0;
		}
	}
}

void PlayerSpaceship::render() {
	VGCVector index(0, 0);
	VGCAdjustment adjustment(0.5, 0.5);
	VGCDisplay::renderImage(image, index, mPosition, adjustment);
}

void PlayerSpaceship::initialize() {
	image = VGCDisplay::openImage(filename, 1, 1);
}

void PlayerSpaceship::finalize() {
	VGCDisplay::closeImage(image);
}


VGCVector PlayerSpaceship::getPosition() const {
	return mPosition;
}

int PlayerSpaceship::getSize() const {
	return image_size;
}

void PlayerSpaceship::destroy(Entity *ent) {
	hud->giveDamage(10);
}

string PlayerSpaceship::getType() const {
	return mType;
}