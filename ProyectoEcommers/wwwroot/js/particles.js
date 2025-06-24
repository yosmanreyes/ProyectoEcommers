document.addEventListener('DOMContentLoaded', () => {
    const particlesContainer = document.getElementById('particles-container');
    const numParticles = 50; // Cantidad de partículas

    function createParticle() {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particlesContainer.appendChild(particle);

        // Posición aleatoria
        const size = Math.random() * 5 + 2; // Tamaño entre 2px y 7px
        particle.style.width = `${size}px`;
        particle.style.height = `${size}px`;

        const x = Math.random() * window.innerWidth;
        const y = Math.random() * window.innerHeight;
        particle.style.left = `${x}px`;
        particle.style.top = `${y}px`;

        // Retraso de animación aleatorio para que no todas destellen a la vez
        particle.style.animationDelay = `${Math.random() * 2}s`;

        // Eliminar la partícula después de un tiempo para evitar sobrecarga (opcional, si no usas 'infinite' en CSS)
        // setTimeout(() => {
        //     particle.remove();
        // }, 2000); // 2 segundos, igual que la duración de la animación
    }

    // Crear un número inicial de partículas
    for (let i = 0; i < numParticles; i++) {
        createParticle();
    }

    // Opcional: Crear nuevas partículas periódicamente para un efecto continuo
    // setInterval(createParticle, 500); // Crea una nueva partícula cada 0.5 segundos

    // Opcional: Ajustar la posición de las partículas al cambiar el tamaño de la ventana
    window.addEventListener('resize', () => {
        // Podrías re-generar o re-posicionar las partículas existentes aquí
        // Para simplicidad, este ejemplo no lo implementa, pero es una consideración
    });
});