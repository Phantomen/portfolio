#include "Explosion.h"

using namespace std;

static const int image_size = 32;
static const int HEIGHT = 32;
static const int WIDTH = 32;

static const string filename = "Explosion.png";
static VGCImage image;

static int lifetime = 30;


Explosion::Explosion(VGCVector spwPos) : mPosition(spwPos) {
	mIsDead = false;
	mCurrentLife = 0;
}


Explosion::~Explosion() {
}

void Explosion::update() {
	if (mCurrentLife >= lifetime) {
		mIsDead = true;
	}
	else
		mCurrentLife = mCurrentLife + 1;
}

void Explosion::render() {
	VGCVector index(0, 0);
	VGCAdjustment adjustment(0.5, 0.5);
	VGCDisplay::renderImage(image, index, mPosition, adjustment);
}

void Explosion::initialize() {
	image = VGCDisplay::openImage(filename, 1, 1);
}

void Explosion::finalize() {
	VGCDisplay::closeImage(image);
}

void Explosion::destroy(Explosion *exp) {
	delete exp;
}

bool Explosion::getIsDead() {
	return mIsDead;
}