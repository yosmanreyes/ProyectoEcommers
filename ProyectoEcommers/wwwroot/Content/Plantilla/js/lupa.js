document.addEventListener("DOMContentLoaded", function () {
    const img = document.getElementById("product-image");
    const lens = document.getElementById("lens");

    lens.style.backgroundImage = `url('${img.src}')`;
    lens.style.backgroundSize = `${img.width * 2}px ${img.height * 2}px`;

    img.addEventListener("mousemove", moveLens);
    lens.addEventListener("mousemove", moveLens);
    img.addEventListener("mouseleave", () => lens.style.visibility = "hidden");

    function moveLens(e) {
        lens.style.visibility = "visible";
        const pos = getCursorPos(e);
        const x = pos.x - (lens.offsetWidth / 2);
        const y = pos.y - (lens.offsetHeight / 2);
        if (x > img.width - lens.offsetWidth) { x = img.width - lens.offsetWidth; }
        if (x < 0) { x = 0; }
        if (y > img.height - lens.offsetHeight) { y = img.height - lens.offsetHeight; }
        if (y < 0) { y = 0; }
        lens.style.left = x + "px";
        lens.style.top = y + "px";
        lens.style.backgroundPosition = `-${x * 2}px -${y * 2}px`;
    }

    function getCursorPos(e) {
        const rect = img.getBoundingClientRect();
        const x = e.pageX - rect.left - window.pageXOffset;
        const y = e.pageY - rect.top - window.pageYOffset;
        return { x: x, y: y };
    }
});
