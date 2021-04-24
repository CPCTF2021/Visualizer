let traPProgress = (wrapper, image) => {
  return (progress) => {
    const x = -Math.floor(progress * 48) % 8 * 600;
    const y = -Math.floor(progress * 8) * 338;
    image.style.top = `${y}px`;
    image.style.left = `${x}px`;
    // if(wrapper == 1) wrapper.style.display = 'none'; 
  }
}