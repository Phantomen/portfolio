#include "Vectors.h"

using namespace std;


Vectors::Vectors() :
	mBulletsPlayer(), mBulletsInvaders() , mPlayer(), mInvaders(), mExplosions(){
}


Vectors::~Vectors() {
}

void Vectors::update(/*vector<Entity*> invaders, vector<Entity*> player*/) {	//Ska stå invaders* resp player* mellan <> PS gör om invaderbox.h till Invaders.h som alla invaders har
	//update Bullets
	for (EntityVector::size_type index = 0; index < mBulletsPlayer.size(); index++) {
		mBulletsPlayer[index]->update();
	}

	for (EntityVector::size_type index = 0; index < mBulletsInvaders.size(); index++) {
		mBulletsInvaders[index]->update();
	}

	//Update Entitys
	for (EntityVector::size_type index = 0; index < mPlayer.size(); index++) {
		mPlayer[index]->update();
	}

	for (EntityVector::size_type index = 0; index < mInvaders.size(); index++) {
		mInvaders[index]->update();
	}

	//Update Explosions
	for (ExplosionVector::size_type index = 0; index < mExplosions.size(); index++) {
		mExplosions.at(index)->update();
	}


	checkBulletOutOfBounds(0 ,mBulletsPlayer);
	checkBulletOutOfBounds(0, mBulletsInvaders);

	checkCollision(0, mBulletsPlayer, mInvaders);

	checkCollision(0, mBulletsInvaders, mPlayer);
	checkCollision(0, mPlayer, mInvaders);

	checkExplosions(0, mExplosions);
}

void Vectors::render() {
	//render Bullets
	for (EntityVector::size_type index = 0; index < mBulletsPlayer.size(); index++) {
		mBulletsPlayer[index]->render();
	}

	for (EntityVector::size_type index = 0; index < mBulletsInvaders.size(); index++) {
		mBulletsInvaders[index]->render();
	}


	//render Entitys
	for (EntityVector::size_type index = 0; index < mPlayer.size(); index++) {
		mPlayer[index]->render();
	}

	for (EntityVector::size_type index = 0; index < mInvaders.size(); index++) {
		mInvaders[index]->render();
	}


	//render Explosions
	for (ExplosionVector::size_type index = 0; index < mExplosions.size(); index++) {
		mExplosions.at(index)->render();
	}
}

void Vectors::initialize() {
	Explosion::initialize();
}

void Vectors::finalize() {
	Explosion::finalize();
}

void Vectors::addPlayer(Entity *player) {
	mPlayer.push_back(player);
}

void Vectors::addBulletPlayer(Entity *bullet) {
	mBulletsPlayer.push_back(bullet);
}

void Vectors::addBulletInvaders(Entity *bullet) {
	mBulletsInvaders.push_back(bullet);
}

void Vectors::addInvader(Entity *invader) {
	mInvaders.push_back(invader);
}

void Vectors::addExplosion(Explosion *explosion) {
	mExplosions.push_back(explosion);
}


void Vectors::checkCollision(EntityVector::size_type startIndex, EntityVector &entVec1, EntityVector &entVec2) {
	bool removeEntitys = false;

	for (EntityVector::size_type i1 = startIndex; i1 < entVec1.size(); i1++) {
		for (EntityVector::size_type i2 = 0; i2 < entVec2.size(); i2++) {
			//Checks if x collide on the right side
			if ((entVec1.at(i1)->getPosition().getX() + (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getX() + (entVec2.at(i2)->getSize() / 2))
				 &&
				(entVec1.at(i1)->getPosition().getX() + (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getX() - (entVec2.at(i2)->getSize() / 2))) {
				//Checks if y "collide" bottom side
				if ((entVec1.at(i1)->getPosition().getY() + (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getY() + (entVec2.at(i2)->getSize() / 2))
					&&
					(entVec1.at(i1)->getPosition().getY() + (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getY() - (entVec2.at(i2)->getSize() / 2))) {

					removeEntitys = true;

					if ((entVec1[i1]->getType() == "Player" && entVec2[i2]->getType() == "Invader")
						||
						(entVec1[i1]->getType() == "Invader" && entVec2[i2]->getType() == "Player")) {

						if (entVec1[i1]->getType() == "Player") {
							entVec1[i1]->destroy(entVec1[i1]);
						}
						else {
							entVec2[i2]->destroy(entVec2[i2]);
						}
					}

					if (entVec1.at(i1)->getType() == "Player") {
						entVec1.at(i1)->destroy(entVec1.at(i1));
					}

					else {
						entVec1.at(i1)->destroy(entVec1.at(i1));
						entVec1.erase(entVec1.begin() + i1);
					}
					//entVec1.erase(entVec1.begin() + i1);

					if (entVec2.at(i2)->getType() == "Player") {
						entVec2.at(i2)->destroy(entVec2.at(i2));
					}

					else {
						entVec2.at(i2)->destroy(entVec2.at(i2));
						entVec2.erase(entVec2.begin() + i2);
					}
					break;
				}
				//checks if y "collide" top side
				else if ((entVec1.at(i1)->getPosition().getY() - (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getY() + (entVec2.at(i2)->getSize() / 2))
					&&
					(entVec1.at(i1)->getPosition().getY() - (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getY() - (entVec2.at(i2)->getSize() / 2))) {

					removeEntitys = true;

					if ((entVec1[i1]->getType() == "Player" && entVec2[i2]->getType() == "Invader")
						||
						(entVec1[i1]->getType() == "Invader" && entVec2[i2]->getType() == "Player")) {

						if (entVec1[i1]->getType() == "Player") {
							entVec1[i1]->destroy(entVec1[i1]);
						}
						else {
							entVec2[i2]->destroy(entVec2[i2]);
						}
					}

					if (entVec1.at(i1)->getType() == "Player") {
						entVec1.at(i1)->destroy(entVec1.at(i1));
					}

					else {
						entVec1.at(i1)->destroy(entVec1.at(i1));
						entVec1.erase(entVec1.begin() + i1);
					}
					//entVec1.erase(entVec1.begin() + i1);

					if (entVec2.at(i2)->getType() == "Player") {
						entVec2.at(i2)->destroy(entVec2.at(i2));
					}

					else {
						entVec2.at(i2)->destroy(entVec2.at(i2));
						entVec2.erase(entVec2.begin() + i2);
					}
					break;
				}
			}

			//Checks if x collide on the left side
			else if ((entVec1.at(i1)->getPosition().getX() - (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getX() + (entVec2.at(i2)->getSize() / 2))
				&&
				(entVec1.at(i1)->getPosition().getX() - (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getX() - (entVec2.at(i2)->getSize() / 2))) {
				//Checks if y "collide" bottom side
				if ((entVec1.at(i1)->getPosition().getY() + (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getY() + (entVec2.at(i2)->getSize() / 2))
					&&
					(entVec1.at(i1)->getPosition().getY() + (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getY() - (entVec2.at(i2)->getSize() / 2))) {

					removeEntitys = true;

					if ((entVec1[i1]->getType() == "Player" && entVec2[i2]->getType() == "Invader")
						||
						(entVec1[i1]->getType() == "Invader" && entVec2[i2]->getType() == "Player")) {

						if (entVec1[i1]->getType() == "Player") {
							entVec1[i1]->destroy(entVec1[i1]);
						}
						else {
							entVec2[i2]->destroy(entVec2[i2]);
						}
					}

					if (entVec1.at(i1)->getType() == "Player") {
						entVec1.at(i1)->destroy(entVec1.at(i1));
					}

					else {
						entVec1.at(i1)->destroy(entVec1.at(i1));
						entVec1.erase(entVec1.begin() + i1);
					}
					//entVec1.erase(entVec1.begin() + i1);

					if (entVec2.at(i2)->getType() == "Player") {
						entVec2.at(i2)->destroy(entVec2.at(i2));
					}

					else {
						entVec2.at(i2)->destroy(entVec2.at(i2));
						entVec2.erase(entVec2.begin() + i2);
					}
					break;
				}
				//checks if y "collide" top side
				else if ((entVec1.at(i1)->getPosition().getY() - (entVec1.at(i1)->getSize() / 2)) <= (entVec2.at(i2)->getPosition().getY() + (entVec2.at(i2)->getSize() / 2))
					&&
					(entVec1.at(i1)->getPosition().getY() - (entVec1.at(i1)->getSize() / 2)) >= (entVec2.at(i2)->getPosition().getY() - (entVec2.at(i2)->getSize() / 2))) {

					removeEntitys = true;

					if ((entVec1[i1]->getType() == "Player" && entVec2[i2]->getType() == "Invader")
						||
						(entVec1[i1]->getType() == "Invader" && entVec2[i2]->getType() == "Player")) {

						if (entVec1[i1]->getType() == "Player") {
							entVec1[i1]->destroy(entVec1[i1]);
						}
						else {
							entVec2[i2]->destroy(entVec2[i2]);
						}
					}

					if (entVec1.at(i1)->getType() == "Player") {
						entVec1.at(i1)->destroy(entVec1.at(i1));
					}

					else {
						entVec1.at(i1)->destroy(entVec1.at(i1));
						entVec1.erase(entVec1.begin() + i1);
					}
					//entVec1.erase(entVec1.begin() + i1);

					if (entVec2.at(i2)->getType() == "Player") {
						entVec2.at(i2)->destroy(entVec2.at(i2));
					}

					else {
						entVec2.at(i2)->destroy(entVec2.at(i2));
						entVec2.erase(entVec2.begin() + i2);
					}
					break;
				}
			}
		}

		if (removeEntitys != false) {
			startIndex = i1;
			break;
		}
	}

	if (removeEntitys != false)
		checkCollision(startIndex, entVec1, entVec2);
}



void Vectors::checkExplosions(ExplosionVector::size_type startIndex, ExplosionVector &explVec) {
	bool explRemoved = false;
	for (ExplosionVector::size_type i = startIndex; i < explVec.size(); i++) {
		if (explVec.at(i)->getIsDead() == true) {
			startIndex = i;
			explVec.at(i)->destroy(explVec.at(i));
			explVec.erase(explVec.begin() + i);
			break;
		}
	}

	if (explRemoved != false) {
		checkExplosions(startIndex, explVec);
	}
}


void Vectors::checkBulletOutOfBounds(EntityVector::size_type startIndex, EntityVector &bulVec) {
	bool removeBullet = false;

	for (EntityVector::size_type index = startIndex; index < bulVec.size(); index++) {
		if (bulVec.at(index)->getPosition().getX() <= -bulVec.at(index)->getSize() / 2) {
			removeBullet = true;
			bulVec.at(index)->destroy(bulVec.at(index));
			bulVec.erase(bulVec.begin() + index);
			startIndex = index;
			break;
		}

		else if (bulVec.at(index)->getPosition().getX() >= VGCDisplay::getWidth() + (bulVec.at(index)->getSize() / 2)) {
			removeBullet = true;
			bulVec.at(index)->destroy(bulVec.at(index));
			bulVec.erase(bulVec.begin() + index);
			startIndex = index;
			break;
		}

		else if (bulVec.at(index)->getPosition().getY() <= -bulVec.at(index)->getSize() / 2) {
			removeBullet = true;
			bulVec.at(index)->destroy(bulVec.at(index));
			bulVec.erase(bulVec.begin() + index);
			startIndex = index;
			break;
		}

		else if (bulVec.at(index)->getPosition().getY() >= VGCDisplay::getHeight() + (bulVec.at(index)->getSize() / 2)) {
			removeBullet = true;
			bulVec.at(index)->destroy(bulVec.at(index));
			bulVec.erase(bulVec.begin() + index);
			startIndex = index;
			break;
		}
	}

	if (removeBullet != false)
		checkBulletOutOfBounds(startIndex, bulVec);
}


void Vectors::destroy() {
	while (!mBulletsPlayer.empty()) {
		delete mBulletsPlayer.back();
		mBulletsPlayer.pop_back();
	}

	while (!mBulletsInvaders.empty()) {
		delete mBulletsInvaders.back();
		mBulletsInvaders.pop_back();
	}

	while (!mPlayer.empty()) {
		delete mPlayer.back();
		mPlayer.pop_back();
	}

	while (!mInvaders.empty()) {
		delete mInvaders.back();
		mInvaders.pop_back();
	}

	while (!mExplosions.empty()) {
		delete mExplosions.back();
		mExplosions.pop_back();
	}
}