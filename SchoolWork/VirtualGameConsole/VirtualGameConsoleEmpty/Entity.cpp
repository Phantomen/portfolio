#include "Entity.h"

Entity::Entity() {
}

Entity::~Entity() {
}

VGCVector getPosition() const {
	return mPosition;
}

int getSize() const {
	return mSize;
}

void destroy(Entity *ent) {
	destroy ent;
}

std::string getType() const {

}

bool isAlive() const {
	return mIsAlive;
}