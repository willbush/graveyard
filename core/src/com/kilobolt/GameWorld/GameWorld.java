package com.kilobolt.GameWorld;

import com.badlogic.gdx.math.Intersector;
import com.badlogic.gdx.math.Rectangle;
import com.kilobolt.GameObjects.Bird;
import com.kilobolt.GameObjects.ScrollHandler;
import com.kilobolt.ZBHelpers.AssetLoader;

public class GameWorld {
    private final Bird bird;
    private final ScrollHandler scroller;
    private Rectangle ground;

    public GameWorld(final int midPointY) {
        bird = new Bird(33, midPointY - 5, 17, 12);
        final int distanceBelowMidPoint = 66 + midPointY;
        scroller = new ScrollHandler(distanceBelowMidPoint);
        ground = new Rectangle(0, midPointY + 66, 136, 11);
    }

    public final void update(final float delta) {
        bird.update(delta);
        scroller.update(delta);
        checkObjectCollision();
        checkGroundCollision();
    }

    private void checkObjectCollision() {
        if (scroller.collides(bird)) {
            scroller.stop();
            AssetLoader.getDead().play();
        }
    }

    private void checkGroundCollision() {
        if (Intersector.overlaps(bird.getCollisionCircle(), ground)) {
            scroller.stop();
            bird.die();
            bird.decelerate();
        }
    }

    public final Bird getBird() {
        return bird;
    }

    public final ScrollHandler getScroller() {
        return scroller;
    }
}
