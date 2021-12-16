#include "Bullet.h"

using namespace std;
static const int image_size = 8;
static const string filename = "Bullet.png";
static VGCImage image;


Bullet::Bullet(VGCVector shotOriginPosition, int speedY, int speedX) :
mPosition(shotOriginPosition){
	mSpeedY = speedY;
	mSpeedX = speedX;
}



Bullet::~Bullet() {
}

void Bullet::update() {
	int y = mPosition.getY();
	y += mSpeedY;
	mPosition.setY(y);

	int x = mPosition.getX();
	x += mSpeedX;
	mPosition.setX(x);
}

void Bullet::render() {
	VGCVector index(0, 0);
	VGCAdjustment adjustment(0.5, 0.5);
	VGCDisplay::renderImage(image, index, mPosition, adjustment);
}

void Bullet::initialize() {
	image = VGCDisplay::openImage(filename, 1, 1);
}

void Bullet::finalize() {
	VGCDisplay::closeImage(image);
}


VGCVector Bullet::getPosition() const{
	return mPosition;
}

int Bullet::getSize() const{
	return image_size;
}


void Bullet::destroy(Entity *bullet) {
	delete bullet;
}

string Bullet::getType() const {
	return mType;
}